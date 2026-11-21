using ECommerce.Application.Contracts.Payments;

namespace ECommerce.Application.Features.Payments.Commands.PaymobWebhook;

internal sealed class PaymobWebhookCommandHandler(IUnitOfWork unitOfWork, IPaymobService paymobService)
    : IRequestHandler<PaymobWebhookCommand, Result>
{
    public async Task<Result> Handle(PaymobWebhookCommand request, CancellationToken cancellationToken)
    {
        var transaction = request.Transaction;

        if (!paymobService.IsValidHmac(transaction, request.Hmac))
            return PaymentErrors.InvalidSignature;

        if (transaction.Pending)
            return Result.Success();

        if (!Guid.TryParse(transaction.Order.MerchantOrderId, out var paymentId))
            return PaymentErrors.NotFound;

        var paymentRepository = unitOfWork.Repository<Payment>();
        var orderRepository = unitOfWork.Repository<Order>();

        var info = await paymentRepository.GetByPredicateProjectAsync(
            x => x.Id == paymentId,
            x => new PaymentWebhookInfo(
                x.Id,
                x.RowVersion,
                x.Status,
                x.Method,
                x.Amount,
                x.Order.Id,
                x.Order.RowVersion,
                x.Order.Status,
                x.Order.PaymentMethod
            ), cancellationToken);

        if (info is null)
            return PaymentErrors.NotFound;

        if (info.Status is PaymentStatus.Succeeded or PaymentStatus.Failed)
            return Result.Success();

        if (transaction.AmountCents != info.Amount.ToCents())
            return PaymentErrors.AmountMismatch;

        var payment = Payment.CreateStub(info.Id, info.RowVersion, info.Method, info.Status);
        var order = Order.CreateStub(info.OrderId, info.OrderRowVersion, info.OrderStatus, info.OrderPaymentMethod);

        if (transaction.Success)
        {
            paymentRepository.PartialUpdate(payment, payment.MarkSucceeded(transaction.Id.ToString(), DateTimeOffset.UtcNow));
            orderRepository.PartialUpdate(order, order.ConfirmPayment());
        }
        else
        {
            paymentRepository.PartialUpdate(payment, payment.MarkFailed(transaction.Id.ToString(), transaction.Data?.Message));

            if (info.OrderStatus == OrderStatus.PendingPayment)
                orderRepository.PartialUpdate(order, order.MarkPaymentFailed());
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
