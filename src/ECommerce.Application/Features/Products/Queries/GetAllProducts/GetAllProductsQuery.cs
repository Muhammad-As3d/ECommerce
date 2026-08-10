using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Products;

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(
    Guid CategoryId,
    FiltersRequest Spec
)
: IRequest<Result<PaginatedList<ProductResponse>>>;
