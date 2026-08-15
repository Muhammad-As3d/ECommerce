using ECommerce.Domain.Enums;

namespace ECommerce.Application.Contracts.Orders;

public record CancelOrderResponse(
    Guid OrderId,
    string OrderNumber,
    OrderStatus Status,
    string? CancellationReason,
    DateTimeOffset? CancelledAt);