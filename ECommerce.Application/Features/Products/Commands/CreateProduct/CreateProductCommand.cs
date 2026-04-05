using ECommerce.Application.Abstractions.Results;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price,
    int CategoryId
    //string CreatedById
    ) : IRequest<Result<int>>;
