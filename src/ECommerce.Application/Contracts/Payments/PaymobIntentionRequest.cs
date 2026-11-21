namespace ECommerce.Application.Contracts.Payments;

public record PaymobIntentionRequest(
    Guid PaymentId,
    long AmountCents,
    string Currency,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Street,
    string City,
    string State,
    string Country
);
