namespace ECommerce.Infrastructure.Clients;

// Paymob wire format (snake_case via PaymobJsonContext). Kept in Infrastructure: Application never sees it.

public sealed record PaymobIntentionBody(
    long Amount,
    string Currency,
    long[] PaymentMethods,
    PaymobItem[] Items,
    PaymobBillingData BillingData,
    string SpecialReference,
    string NotificationUrl,
    string RedirectionUrl);

public sealed record PaymobItem(
    string Name,
    long Amount,
    string Description,
    int Quantity);

public sealed record PaymobBillingData(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Street,
    string City,
    string State,
    string Country,
    string Building = "NA",
    string Floor = "NA",
    string Apartment = "NA",
    string PostalCode = "NA");

public sealed record PaymobIntentionResponse(string Id, string ClientSecret);
