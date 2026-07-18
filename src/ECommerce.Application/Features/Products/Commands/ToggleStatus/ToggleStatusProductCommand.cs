namespace ECommerce.Application.Features.Products.Commands.ToggleStatus;

public record ToggleStatusProductCommand(
    Guid CategoryId,
    Guid Id
) : IRequest<Result>;