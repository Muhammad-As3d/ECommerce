using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Specifications.CategorySpecifications;

namespace ECommerce.Application.Features.Categories.Queries.GetCategoryProducts;

public record GetCategoryProductsQuery(Guid Id) : IRequest<Result<CategoryProductsResponse>>;

public class GetCategoryProductsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCategoryProductsQuery, Result<CategoryProductsResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CategoryProductsResponse>> Handle(GetCategoryProductsQuery request, CancellationToken cancellationToken)
    {
        var spec = new CategoryWithProductsSpecification(request.Id);

        var categoryProducts = await _unitOfWork
            .Repository<Category>()
            .GetBySpecProjectAsync<CategoryProductsResponse>(spec, cancellationToken);

        if (categoryProducts is null)
            return Result.Failure<CategoryProductsResponse>(CategoryErrors.NotFound(request.Id));

        return Result.Success(categoryProducts);
    }
}