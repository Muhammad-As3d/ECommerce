using ECommerce.Application.Contracts.Payments;

namespace ECommerce.Application.Features.Payments.Commands.CreatePayment;

internal sealed class CreatePaymentCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, IPaymobService paymobService)
    : IRequestHandler<CreatePaymentCommand, Result<CreatePaymentResponse>>
{
    public async Task<Result<CreatePaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork
            .Repository<Order>()
            .GetByPredicateProjectAsync(x => x.Id == request.OrderId && x.UserId == currentUser.Id,
            x => new OrderPaymentInfo(
                x.Id,
                x.Status,
                x.PaymentMethod,
                x.TotalAmount,
                x.Currency,
                x.ShippingAddress.FullName,
                x.ShippingAddress.PhoneNumber,
                x.ShippingAddress.Street,
                x.ShippingAddress.City,
                x.ShippingAddress.Governorate,
                x.ShippingAddress.Country
            ), cancellationToken);

        if (order is null)
            return OrderErrors.NotFound(request.OrderId);

        if (order.PaymentMethod != PaymentMethod.Card)
            return PaymentErrors.NotCardOrder;

        if (order.Status is not (OrderStatus.PendingPayment or OrderStatus.PaymentFailed))
            return PaymentErrors.OrderNotPayable;

        var payment = Payment.CreatePaymob(order.Id, order.TotalAmount, order.Currency);

        var (firstName, lastName) = SplitName(order.FullName);

        var paymobIntentionRequest = new PaymobIntentionRequest(
            payment.Id,
            order.TotalAmount.ToCents(),
            order.Currency,
            firstName,
            lastName,
            currentUser.Email,
            order.PhoneNumber,
            order.Street,
            order.City,
            order.Governorate,
            order.Country);

        var intention = await paymobService.CreateIntentionAsync(paymobIntentionRequest, cancellationToken);

        if (intention is null)
            return PaymentErrors.ProviderFailed;

        payment.AttachIntention(intention.IntentionId);

        await unitOfWork.Repository<Payment>().AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreatePaymentResponse(payment.Id, intention.ClientSecret, intention.CheckoutUrl));
    }

    private static (string FirstName, string LastName) SplitName(string fullName)
    {
        var parts = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        return parts.Length switch
        {
            0 => ("NA", "NA"),
            1 => (parts[0], "NA"),
            _ => (parts[0], parts[1])
        };
    }
}
