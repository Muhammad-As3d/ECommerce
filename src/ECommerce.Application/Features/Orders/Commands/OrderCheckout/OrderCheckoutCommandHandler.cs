using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Specifications.CartSpecification;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Orders.Commands.OrderCheckout;

internal sealed class OrderCheckoutCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser,
    IOrderNumberGenerator orderNumberGenerator)
    : IRequestHandler<OrderCheckoutCommand, Result<OrderResponse>>
{
    private const string Currency = "EGP";
    private const decimal ShippingFee = 50m;

    public async Task<Result<OrderResponse>> Handle(OrderCheckoutCommand request, CancellationToken cancellationToken)
    {
        if (request.PaymentMethod != PaymentMethod.CashOnDelivery)
            return Result.Failure<OrderResponse>(OrderErrors.UnsupportedPaymentMethod);

        if (!string.IsNullOrWhiteSpace(request.CouponCode))
            return Result.Failure<OrderResponse>(OrderErrors.CouponNotSupported);

        var cartItems = await GetCartItemsAsync(cancellationToken);

        if (cartItems.Count == 0)
            return Result.Failure<OrderResponse>(CartErrors.Empty);

        //var addressInfo = await unitOfWork.Repository<Address>().GetByPredicateProjectAsync<ShippingAddressInfo>(
        //    x => x.Id == request.ShippingAddressId && x.UserId == currentUser.Id, cancellationToken);

        var addressInfo = await unitOfWork
            .Repository<Address>()
            .GetByPredicateProjectAsync(x => x.Id == request.ShippingAddressId && x.UserId == currentUser.Id,
            x => new ShippingAddressInfo(
                x.FullName,
                x.PhoneNumber,
                x.Street,
                x.City,
                x.Governorate,
                x.Country,
                x.PostalCode
            ), cancellationToken);

        if (addressInfo is null)
            return Result.Failure<OrderResponse>(AddressErrors.AddressNotFound);

        var productRepository = (IProductRepository)unitOfWork.Repository<Product>();
        var productIds = cartItems.Select(x => x.ProductId).Distinct().ToArray();
        var productsInfo = await productRepository.GetCheckoutInfoByIdsAsync(productIds, cancellationToken);

        if (productsInfo.Count != productIds.Length)
            return Result.Failure<OrderResponse>(OrderErrors.ProductNotAvailable);

        var productsById = productsInfo.ToDictionary(x => x.Id);

        foreach (var item in cartItems)
        {
            if (!productsById.TryGetValue(item.ProductId, out var product) || product.IsDeleted || !product.IsActive)
                return Result.Failure<OrderResponse>(OrderErrors.ProductNotAvailable);

            if (item.Quantity <= 0 || product.Stock < item.Quantity)
                return Result.Failure<OrderResponse>(CartErrors.InsufficientStock);
        }

        var shippingAddress = ShippingAddressSnapshot.Create(
            addressInfo.FullName,
            addressInfo.PhoneNumber,
            addressInfo.Street,
            addressInfo.City,
            addressInfo.Governorate,
            addressInfo.Country,
            addressInfo.PostalCode);

        var orderNumber = await orderNumberGenerator.GenerateAsync(cancellationToken);
        var order = Order.Create(currentUser.Id, orderNumber, request.PaymentMethod, shippingAddress, Currency, ShippingFee);

        foreach (var item in cartItems)
        {
            var product = productsById[item.ProductId];
            order.AddItem(product.Id, product.Name, product.Sku, product.Price, item.Quantity);
        }

        var payment = Payment.CreateCash(order.Id, order.TotalAmount, order.Currency);
        order.AddPayment(payment);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var item in cartItems)
            {
                var decreased = await productRepository.TryDecreaseStockAsync(item.ProductId, item.Quantity, cancellationToken);

                if (!decreased)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure<OrderResponse>(CartErrors.InsufficientStock);
                }
            }

            await unitOfWork.Repository<Order>().AddAsync(order, cancellationToken);

            var initialHistory = OrderStatusHistory.Create(
                order.Id,
                null,
                order.Status,
                currentUser.Id,
                null);

            await unitOfWork.Repository<OrderStatusHistory>().AddAsync(initialHistory, cancellationToken);

            await unitOfWork.Repository<CartItem>().DeleteAsync(x => x.Cart.UserId == currentUser.Id, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        return Result.Success(new OrderResponse(
            order.Id,
            order.OrderNumber,
            order.Status.ToString(),
            order.TotalAmount,
            order.Currency,
            new PaymentResponse(payment.Id, payment.Method.ToString(), payment.Status.ToString(),
            null, payment.Amount, payment.Currency)));
    }

    private async Task<List<CartItemCheckoutInfo>> GetCartItemsAsync(CancellationToken cancellationToken)
    {
        var specification = new OrderItemsSpecification(currentUser.Id);
        var items = await unitOfWork.Repository<CartItem>().GetAllSpecProjectAsync<CartItemCheckoutInfo>(specification, cancellationToken);

        return items?.ToList() ?? [];
    }
}
