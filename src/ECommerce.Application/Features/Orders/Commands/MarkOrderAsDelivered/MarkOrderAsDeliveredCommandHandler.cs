namespace ECommerce.Application.Features.Orders.Commands.MarkOrderAsDelivered;

internal sealed class MarkOrderAsDeliveredCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<MarkOrderAsDeliveredCommand, Result<MarkOrderAsDeliveredResponse>>
{
    public async Task<Result<MarkOrderAsDeliveredResponse>> Handle(MarkOrderAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        var orderRepository = unitOfWork.Repository<Order>();

        var orderInfo = await orderRepository.GetByPredicateProjectAsync(
            order => order.Id == request.OrderId,
            order => new MarkOrderAsDeliveredInfo(
                order.Id,
                order.Status,
                order.PaymentMethod,
                order.RowVersion,
                order.Payments
                    .Where(payment => payment.Method == PaymentMethod.CashOnDelivery)
                    .Select(payment => new CashPaymentInfo(
                        payment.Id,
                        payment.Method,
                        payment.Status,
                        payment.RowVersion))
                    .FirstOrDefault()),
            cancellationToken);

        if (orderInfo is null)
            return Result.Failure<MarkOrderAsDeliveredResponse>(OrderErrors.NotFound(request.OrderId));

        if (orderInfo.Status != OrderStatus.Shipped)
            return Result.Failure<MarkOrderAsDeliveredResponse>(OrderErrors.StatusNotShipped);

        if (orderInfo.PaymentMethod == PaymentMethod.CashOnDelivery && orderInfo.CashPayment is null)
            return Result.Failure<MarkOrderAsDeliveredResponse>(OrderErrors.CashPaymentNotFound);

        var order = Order.CreateStub(orderInfo.Id, orderInfo.RowVersion, orderInfo.Status);
        var updatedOrderProperties = order.MarkDelivered();
        orderRepository.PartialUpdate(order, updatedOrderProperties);

        if (orderInfo.CashPayment is not null)
        {
            var payment = Payment.CreateStub(
                orderInfo.CashPayment.Id,
                orderInfo.CashPayment.RowVersion,
                orderInfo.CashPayment.Method,
                orderInfo.CashPayment.Status);

            var updatedPaymentProperties = payment.MarkCashCollected();
            unitOfWork.Repository<Payment>().PartialUpdate(payment, updatedPaymentProperties);
        }

        var history = OrderStatusHistory.Create(order.Id, orderInfo.Status, OrderStatus.Delivered, currentUser.Id, null);

        await unitOfWork.Repository<OrderStatusHistory>().AddAsync(history, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new MarkOrderAsDeliveredResponse(
            order.Id,
            order.Status,
            order.DeliveredAt!.Value));
    }
}
