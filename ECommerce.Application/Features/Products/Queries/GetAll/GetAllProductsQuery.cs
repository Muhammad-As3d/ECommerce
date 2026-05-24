using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Specifications.ProductSpecifications;

namespace ECommerce.Application.Features.Products.Queries.GetAll;

public record GetAllProductsQuery(int CategoryId, SpecificationRequest Spec) : IRequest<Result<PaginatedList<ProductResponse>>>;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllProductsQuery, Result<PaginatedList<ProductResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedList<ProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var isCategoryExists = await _unitOfWork
            .Repository<Category>()
            .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!isCategoryExists)
            return Result.Failure<PaginatedList<ProductResponse>>(CategoryErrors.NotFound(request.CategoryId));

        var spec = new ProductSpecification(request.CategoryId, request.Spec);

        var products = await _unitOfWork
            .Repository<Product>()
            .GetAllPaginatedProjectAsync<ProductResponse>(spec, request.Spec.PageNumber, request.Spec.PageSize, cancellationToken);

        return Result.Success(products);
    }
}
