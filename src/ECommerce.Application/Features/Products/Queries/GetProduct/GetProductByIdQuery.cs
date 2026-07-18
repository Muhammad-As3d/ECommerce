using ECommerce.Application.Contracts.Products;

namespace ECommerce.Application.Features.Products.Queries.GetProduct;

public record GetProductByIdQuery(
    Guid CategoryId,
    Guid Id
)
: IRequest<Result<ProductResponse>>;