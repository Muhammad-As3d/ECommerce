using ECommerce.Application.Contracts.Payments;
using ECommerce.Infrastructure.Clients;
using ECommerce.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Refit;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Implementations.Services;

public class PaymobService(IOptions<PaymobSettings> options,
    ILogger<PaymobService> logger, IPaymobClient paymobClient) : IPaymobService
{
    private readonly PaymobSettings _settings = options.Value;

    public async Task<PaymobIntention?> CreateIntentionAsync(PaymobIntentionRequest request, CancellationToken cancellationToken)
    {
        var body = new PaymobIntentionBody(
            Amount: request.AmountCents,
            Currency: request.Currency,
            PaymentMethods: [_settings.CardIntegrationId],
            Items: [],
            BillingData: new PaymobBillingData(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.Street,
                request.City,
                request.State,
                request.Country),
            SpecialReference: request.PaymentId.ToString(),
            NotificationUrl: _settings.NotificationUrl,
            RedirectionUrl: _settings.RedirectionUrl);

        var response = await paymobClient.CreateIntentionAsync(body, cancellationToken);

        if (!response.IsSuccessfulWithContent)
        {
            logger.LogError(response.Error, "Paymob intention failed. Status: {Status}, Body: {Body}",
                response.StatusCode, (response.Error as ApiException)?.Content);
            return null;
        }

        var result = response.Content;

        var checkoutUrl = $"{_settings.BaseUrl}/unifiedcheckout/?publicKey={_settings.PublicKey}&clientSecret={result.ClientSecret}";

        return new PaymobIntention(result.Id, result.ClientSecret, checkoutUrl);
    }

    public bool IsValidHmac(PaymobTransaction t, string hmac)
    {
        if (string.IsNullOrWhiteSpace(hmac))
            return false;

        var data = string.Concat(
            t.AmountCents.ToString(),
            t.CreatedAt,
            t.Currency,
            Bool(t.ErrorOccured),
            Bool(t.HasParentTransaction),
            t.Id.ToString(),
            t.IntegrationId.ToString(),
            Bool(t.Is3dSecure),
            Bool(t.IsAuth),
            Bool(t.IsCapture),
            Bool(t.IsRefunded),
            Bool(t.IsStandalonePayment),
            Bool(t.IsVoided),
            t.Order.Id.ToString(),
            t.Owner.ToString(),
            Bool(t.Pending),
            t.SourceData.Pan,
            t.SourceData.SubType,
            t.SourceData.Type,
            Bool(t.Success));

        var hash = HMACSHA512.HashData(Encoding.UTF8.GetBytes(_settings.HmacSecret), Encoding.UTF8.GetBytes(data));
        var computed = Convert.ToHexStringLower(hash);

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computed),
            Encoding.UTF8.GetBytes(hmac.ToLowerInvariant()));
    }

    private static string Bool(bool value) => value ? "true" : "false";
}
