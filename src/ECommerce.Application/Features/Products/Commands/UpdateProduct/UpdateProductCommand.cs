namespace ECommerce.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int CategoryId,
    int Id,
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price
) : IRequest<Result>;
