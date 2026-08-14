using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class OrderErrors
{
    public static Error NotFound(Guid orderId) =>
        Error.NotFound("Order.NotFound", $"Order with ID '{orderId}' was not found.");

    public static Error UnsupportedPaymentMethod =>
        Error.BadRequest("Order.UnsupportedPaymentMethod", "Only cash on delivery is currently supported.");

    public static Error CouponNotSupported =>
        Error.BadRequest("Order.CouponNotSupported", "Coupons are not supported yet.");

    public static Error ProductNotAvailable =>
        Error.Conflict("Order.ProductNotAvailable", "One or more products are no longer available.");
}
