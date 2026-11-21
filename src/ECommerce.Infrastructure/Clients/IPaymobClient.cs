using Refit;

namespace ECommerce.Infrastructure.Clients;

public interface IPaymobClient
{
    [Post("/v1/intention/")]
    Task<IApiResponse<PaymobIntentionResponse>> CreateIntentionAsync([Body] PaymobIntentionBody body, CancellationToken cancellationToken);
}
