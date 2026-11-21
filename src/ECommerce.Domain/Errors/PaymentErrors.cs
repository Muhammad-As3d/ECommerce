using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class PaymentErrors
{
    public static Error NotCardOrder =>
        Error.BadRequest("Payment.NotCardOrder", "This order is not a card order.");

    public static Error OrderNotPayable =>
        Error.Conflict("Payment.OrderNotPayable", "This order cannot be paid in its current status.");

    public static Error ProviderFailed =>
        Error.Failure("Payment.ProviderFailed", "Payment provider is unavailable, please try again.");

    public static Error InvalidSignature =>
        Error.Unauthorized("Payment.InvalidSignature", "Invalid webhook signature.");

    public static Error NotFound =>
        Error.NotFound("Payment.NotFound", "Payment was not found.");

    public static Error AmountMismatch =>
        Error.Conflict("Payment.AmountMismatch", "Paid amount does not match.");
}
