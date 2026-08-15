using ECommerce.Application.Interfaces.Repositories;

namespace ECommerce.Application.Features.Orders.Commands.CancelOrder;

internal sealed class CancelOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<CancelOrderCommand, Result<CancelOrderResponse>>
{
    public async Task<Result<CancelOrderResponse>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var orderRepository = unitOfWork.Repository<Order>();

        var orderInfo = await orderRepository
            .GetByPredicateProjectAsync(x => x.Id == request.OrderId && x.UserId == currentUser.Id,
            s => new CancelOrderInfo(
                s.Id,
                s.Status,
                s.OrderNumber,
                s.RowVersion,
                s.CancellationReason,
                s.Items.Select(i => new CancelOrderProductInfo(
                    i.ProductId,
                    i.Quantity)).ToList()),
            cancellationToken);

        if (orderInfo is null)
            return Result.Failure<CancelOrderResponse>(OrderErrors.NotFound(request.OrderId));

        var canCancel = orderInfo.Status is OrderStatus.PendingPayment
            or OrderStatus.Confirmed
            or OrderStatus.Processing
            or OrderStatus.PaymentFailed;

        if (!canCancel)
            return Result.Failure<CancelOrderResponse>(OrderErrors.NotAllowedCancel);

        var previousStatus = orderInfo.Status;
        var productRepository = (IProductRepository)unitOfWork.Repository<Product>();

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var order = Order.CreateStub(orderInfo.Id, orderInfo.RowVersion);
        try
        {
            var changedProperties = order.Cancel(request.CancellationReason);
            orderRepository.PartialUpdate(order, changedProperties);

            var history = OrderStatusHistory.Create(orderInfo.Id, previousStatus, OrderStatus.Cancelled, currentUser.Id,
                request.CancellationReason);

            await unitOfWork.Repository<OrderStatusHistory>().AddAsync(history, cancellationToken);

            foreach (var item in orderInfo.ProductInfo)
            {
                if (item.ProductId is Guid productId)
                    await productRepository.IncreaseStockAsync(productId, item.Quantity, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        return Result.Success(new CancelOrderResponse(orderInfo.Id, orderInfo.OrderNumber, OrderStatus.Cancelled,
            order.CancellationReason, order.CancelledAt));
    }
}