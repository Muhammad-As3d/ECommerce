using ECommerce.Application.Contracts.Payments;

namespace ECommerce.Application.Interfaces.Payments;

public interface IPaymentGateway
{
    Task<CreatePaymentIntentResult> CreatePaymentIntentAsync(CreatePaymentIntentRequest request, CancellationToken cancellationToken = default);
    Task CancelPaymentIntentAsync(string paymentIntentId, CancellationToken cancellationToken = default);
}