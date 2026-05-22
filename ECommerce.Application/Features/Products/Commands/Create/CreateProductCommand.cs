using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Products.Commands.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price,
    int CategoryId,
    List<IFormFile> Images

) : IRequest<Result<int>>;
