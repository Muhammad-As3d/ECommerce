using ECommerce.Domain.Abstractions;
using MediatR;

namespace ECommerce.Application.Features.Products.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price,
    int CategoryId
    //string CreatedById
    ) : IRequest<Result<int>>;
