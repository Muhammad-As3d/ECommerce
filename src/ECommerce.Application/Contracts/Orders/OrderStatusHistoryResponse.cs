namespace ECommerce.Application.Contracts.Orders;

public sealed record OrderStatusHistoryResponse(
    Guid OrderId,
    IReadOnlyCollection<OrderStatusHistoryItemResponse> History);

public sealed record OrderStatusHistoryItemResponse(
    OrderStatus Status,
    DateTimeOffset ChangedAt);
