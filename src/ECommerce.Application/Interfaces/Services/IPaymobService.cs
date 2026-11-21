using ECommerce.Application.Contracts.Payments;

namespace ECommerce.Application.Interfaces.Services;

public interface IPaymobService
{
    Task<PaymobIntention?> CreateIntentionAsync(PaymobIntentionRequest request, CancellationToken cancellationToken);

    bool IsValidHmac(PaymobTransaction transaction, string hmac);
}
