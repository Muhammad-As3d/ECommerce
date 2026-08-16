namespace ECommerce.Application.Contracts.Orders;

public sealed record MarkOrderAsShippedResponse(
    Guid Id,
    OrderStatus Status,
    string TrackingNumber,
    string ShippingProvider,
    DateTimeOffset EstimatedDeliveryFrom,
    DateTimeOffset EstimatedDeliveryTo,
    DateTimeOffset ShippedAt);

public sealed record MarkOrderAsShippedInfo(
    Guid Id,
    OrderStatus Status,
    byte[] RowVersion);
