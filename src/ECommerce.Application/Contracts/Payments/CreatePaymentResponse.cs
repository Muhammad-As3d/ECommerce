namespace ECommerce.Application.Contracts.Payments;

public record CreatePaymentResponse(
    Guid PaymentId,
    string ClientSecret,
    string CheckoutUrl
);