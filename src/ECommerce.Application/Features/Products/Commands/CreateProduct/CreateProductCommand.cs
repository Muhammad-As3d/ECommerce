using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    Guid CategoryId,
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price,
    List<IFormFile> Images

) : IRequest<Result<Guid>>;