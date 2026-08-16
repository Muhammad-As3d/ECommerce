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

    public static Error NotAllowedCancel =>
        Error.Conflict("Order.NotAllowedCancel", "You are not allowed to cancel this order.");

    public static Error StatusNotConfirmed =>
        Error.Conflict("Order.StatusNotConfirmed", "The order status is not confirmed.");

    public static Error StatusNotProcessing =>
        Error.Conflict("Order.StatusNotProcessing", "The order status is not processing.");

    public static Error StatusNotShipped =>
        Error.Conflict("Order.StatusNotShipped", "The order status is not shipped.");

    public static Error CashPaymentNotFound =>
        Error.Conflict("Order.CashPaymentNotFound", "The cash payment for this order was not found.");
}
