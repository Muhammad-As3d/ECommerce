namespace ECommerce.Application.Features.Orders.Commands.MarkOrderAsShipped;

public sealed record MarkOrderAsShippedCommand(
    Guid OrderId,
    DateTimeOffset EstimatedDeliveryFrom,
    DateTimeOffset EstimatedDeliveryTo,
    string TrackingNumber,
    string ShippingProvider) : IRequest<Result<MarkOrderAsShippedResponse>>;
