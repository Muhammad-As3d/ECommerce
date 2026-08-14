using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class CheckoutErrors
{
    public static Error CouponNotSupported =>
        Error.BadRequest(
            "Checkout.CouponNotSupported",
            "Coupons are not supported yet.");

    public static Error ProductNotAvailable =>
        Error.Conflict(
            "Checkout.ProductNotAvailable",
            "One or more products are no longer available.");

    public static Error PaymentCreationFailed =>
        Error.Failure(
            "Checkout.PaymentCreationFailed",
            "The payment could not be created.");

    public static Error PriceChanged =>
        Error.Conflict(
            "Checkout.PriceChanged",
            "One or more product prices have changed. Please review your cart.");
}