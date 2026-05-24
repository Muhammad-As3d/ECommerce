using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Features.Products.Commands.Create;

public record CreateProductCommand(
    [property: FromRoute] int categoryId,
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price,
    List<IFormFile> Images

) : IRequest<Result<int>>;
