namespace ECommerce.Application.Contracts.Payments;

public record CreatePaymentIntentRequest(
    Guid OrderId,
    string OrderNumber,
    decimal Amount,
    string Currency,
    string IdempotencyKey);

public sealed record CreatePaymentIntentResult(
    string PaymentIntentId,
    string ClientSecret);