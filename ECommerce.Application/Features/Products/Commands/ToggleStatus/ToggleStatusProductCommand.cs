using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Features.Products.Commands.ToggleStatus;

public record ToggleStatusProductCommand(
    [property: FromRoute] int categoryId,
    int Id
) : IRequest<Result>;
