namespace ECommerce.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid CategoryId,
    Guid Id,
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price
) : IRequest<Result>;
