namespace ECommerce.Application.Contracts.Payments;

public record PaymobIntention(
    string IntentionId,
    string ClientSecret,
    string CheckoutUrl
);
