using ECommerce.Application.Contracts.Addresses;
using ECommerce.Application.Contracts.Carts;
using ECommerce.Application.Contracts.Orders;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Specifications.CartSpecification;

namespace ECommerce.Application.Features.Orders.Commands;

internal class OrderCheckoutCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser,
    IOrderNumberGenerator orderNumberGenerator)
    : IRequestHandler<OrderCheckoutCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(OrderCheckoutCommand request, CancellationToken cancellationToken)
    {
        var itemSpec = new OrderItemsSpecification(currentUser.Id);
        var cartItems = await unitOfWork
            .Repository<CartItem>()
            .GetAllSpecProjectAsync<CartItemResponse>(itemSpec, cancellationToken);

        if (cartItems is null || !cartItems.Any())
            return Result.Failure<OrderResponse>(CartErrors.Empty);

        var shippingAddress = await unitOfWork
            .Repository<Address>()
            .GetByPredicateProjectAsync<AddressResponse>(x => x.Id == request.ShippingAddressId && x.UserId == currentUser.Id,
            cancellationToken);

        if (shippingAddress is null)
            return Result.Failure<OrderResponse>(AddressErrors.AddressNotFound);

        var productRepo = (IProductRepository)unitOfWork.Repository<Product>();
        var productIds = cartItems.Select(x => x.ProductId).ToList();

        var subTotal = cartItems!.Sum(x => x.Subtotal);
        var orderNumber = await orderNumberGenerator.GenerateAsync(cancellationToken);
        var order = Order.Create(currentUser.Id, request.ShippingAddressId, subTotal, orderNumber);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var item in cartItems)
            {
                var decreased = await productRepo.TryDecreaseStockAsync(item.ProductId, item.Quantity, cancellationToken);

                if (!decreased)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure<OrderResponse>(CartErrors.InsufficientStock);
                }

                order.AddItem(OrderItem.Create(order.Id, item.ProductName, item.UnitPrice, item.Quantity));
            }

            await unitOfWork.Repository<Order>().AddAsync(order, cancellationToken);
            await unitOfWork.Repository<OrderItem>().AddRangeAsync(order.Items.ToList(), cancellationToken);

            await unitOfWork.Repository<CartItem>().DeleteAsync(x => x.Cart.UserId == currentUser.Id, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        var response = new OrderResponse(order.Id, order.OrderNumber, order.Status.ToString(), order.CreatedOn,
            order.WithinDays, order.SubTotal, order.DiscountAmount, order.ShippingFee, order.TaxAmount,
            order.TotalAmount, shippingAddress, cartItems);

        return Result.Success(response);
    }
}