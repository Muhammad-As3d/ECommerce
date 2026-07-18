namespace ECommerce.Application.Features.Categories.Commands.ToggleStatus;

public record ToggleStatusCategoryCommand(Guid Id) : IRequest<Result>;