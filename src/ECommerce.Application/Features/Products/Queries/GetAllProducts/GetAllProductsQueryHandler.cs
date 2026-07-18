using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Specifications.ProductSpecifications;

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllProductsQuery, Result<PaginatedList<ProductResponse>>>
{
    public async Task<Result<PaginatedList<ProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var categoryIsExists = await unitOfWork
            .Repository<Category>()
            .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryIsExists)
            return Result.Failure<PaginatedList<ProductResponse>>(CategoryErrors.NotFound(request.CategoryId));

        var spec = new ProductSpecification(categoryId: request.CategoryId, spec: request.Spec);

        var response = await unitOfWork
            .Repository<Product>()
            .GetAllPaginatedProjectAsync<ProductResponse>(spec, request.Spec.PageNumber, request.Spec.PageSize, cancellationToken);

        return Result.Success(response);
    }
}