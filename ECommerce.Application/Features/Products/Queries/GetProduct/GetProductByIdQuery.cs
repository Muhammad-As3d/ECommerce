using ECommerce.Application.Contracts.Products;

namespace ECommerce.Application.Features.Products.Queries.GetProduct;

public record GetProductByIdQuery(
    int CategoryId,
    int Id
    )
    : IRequest<Result<ProductResponse>>;