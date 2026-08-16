namespace ECommerce.Application.Features.Orders.Commands.MarkOrderAsDelivered;

public sealed record MarkOrderAsDeliveredCommand(Guid OrderId)
    : IRequest<Result<MarkOrderAsDeliveredResponse>>;
