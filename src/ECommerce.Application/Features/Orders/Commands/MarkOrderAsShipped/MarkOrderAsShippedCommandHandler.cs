namespace ECommerce.Application.Features.Orders.Commands.MarkOrderAsShipped;

internal sealed class MarkOrderAsShippedCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IRealTimeNotifier realTimeNotifier) : IRequestHandler<MarkOrderAsShippedCommand, Result<MarkOrderAsShippedResponse>>
{
    public async Task<Result<MarkOrderAsShippedResponse>> Handle(MarkOrderAsShippedCommand request, CancellationToken cancellationToken)
    {
        var orderRepository = unitOfWork.Repository<Order>();

        var orderInfo = await orderRepository.GetByPredicateProjectAsync(o => o.Id == request.OrderId,
            o => new MarkOrderAsShippedInfo
            (
                o.Id,
                o.Status,
                o.RowVersion,
                o.UserId
            )
            , cancellationToken);

        if (orderInfo is null)
            return OrderErrors.NotFound(request.OrderId);

        if (orderInfo.Status is not OrderStatus.Processing)
            return OrderErrors.StatusNotProcessing;

        var order = Order.CreateStub(orderInfo.Id, orderInfo.RowVersion, orderInfo.Status);
        var updatedProperties = order.MarkShipped(
            request.EstimatedDeliveryFrom,
            request.EstimatedDeliveryTo,
            request.TrackingNumber,
            request.ShippingProvider);

        orderRepository.PartialUpdate(order, updatedProperties);

        var history = OrderStatusHistory.Create(
            order.Id,
            orderInfo.Status,
            OrderStatus.Shipped,
            currentUser.Id,
            null);

        await unitOfWork.Repository<OrderStatusHistory>().AddAsync(history, cancellationToken);

        var notification = new Notification
        {
            UserId = orderInfo.UserId,
            Title = "Your order has shipped 🚚",
            Message = $"Shipped with {request.ShippingProvider}. Tracking number: {request.TrackingNumber}",
            Type = NotificationType.OrderStatusChanged
        };

        await unitOfWork.Repository<Notification>().AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await realTimeNotifier.SendToUserAsync(orderInfo.UserId, notification, cancellationToken);

        return Result.Success(new MarkOrderAsShippedResponse(
            order.Id,
            order.Status,
            order.TrackingNumber!,
            order.ShippingProvider!,
            order.EstimatedDeliveryFrom!.Value,
            order.EstimatedDeliveryTo!.Value,
            order.ShippedAt!.Value));
    }
}
