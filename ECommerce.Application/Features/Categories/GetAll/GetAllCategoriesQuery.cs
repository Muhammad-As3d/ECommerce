using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Features.Categories.Specifications;

namespace ECommerce.Application.Features.Categories.GetAll;

public record GetAllCategoriesQuery(PageFilters Page) : IRequest<PaginatedList<CategoryResponse>>;

public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCategoriesQuery, PaginatedList<CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<PaginatedList<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var spec = new CategorySpecification(request.Page.SearchValue, null, request.Page.IsDescending);

        return await _unitOfWork
        .Repository<Category>()
        .GetAllPaginatedProjectAsync<CategoryResponse>(spec, request.Page.PageNumber, request.Page.PageSize, cancellationToken);
    }
}
