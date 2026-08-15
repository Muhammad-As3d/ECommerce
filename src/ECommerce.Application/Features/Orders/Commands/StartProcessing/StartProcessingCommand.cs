namespace ECommerce.Application.Features.Orders.Commands.StartProcessing;

public record StartProcessingCommand(Guid OrderId) : IRequest<Result<StartProcessingResponse>>;
