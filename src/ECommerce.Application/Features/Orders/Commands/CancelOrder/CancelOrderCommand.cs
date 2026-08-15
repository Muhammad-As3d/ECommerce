namespace ECommerce.Application.Features.Orders.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId, string CancellationReason) : IRequest<Result<CancelOrderResponse>>;