namespace ECommerce.Application.Features.Products.Commands.ToggleStatus;

public record ToggleStatusProductCommand(
    int CategoryId,
    int Id
) : IRequest<Result>;