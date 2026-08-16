namespace ECommerce.Api.ViewModels.Orders;

public sealed record MarkOrderAsShippedRequest(
    DateTimeOffset EstimatedDeliveryFrom,
    DateTimeOffset EstimatedDeliveryTo,
    string TrackingNumber,
    string ShippingProvider);
