using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Specifications.ProductSpecifications;

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllProductsQuery, PaginatedList<ProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PaginatedList<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductSpecification(categoryId: request.CategoryId, spec: request.Spec);

        return await _unitOfWork
            .Repository<Product>()
            .GetAllPaginatedProjectAsync<ProductResponse>(spec, request.Spec.PageNumber, request.Spec.PageSize, cancellationToken);
    }
}