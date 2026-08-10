using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class OrderErrors
{
    public static Error NotFound(Guid orderId) =>
        Error.NotFound("Order.NotFound", $"Order with ID '{orderId}' was not found.");
}
