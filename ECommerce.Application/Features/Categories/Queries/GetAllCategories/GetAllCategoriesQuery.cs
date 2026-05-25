using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Specifications.CategorySpecifications;

namespace ECommerce.Application.Features.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery(SpecificationRequest Spec)
    : IRequest<PaginatedList<CategoryResponse>>;

public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCategoriesQuery, PaginatedList<CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<PaginatedList<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var spec = new CategorySpecification(request.Spec);

        return await _unitOfWork
        .Repository<Category>()
        .GetAllPaginatedProjectAsync<CategoryResponse>(spec, request.Spec.PageNumber, request.Spec.PageSize,
        cancellationToken);
    }
}
