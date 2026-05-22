using ECommerce.Application.Contracts.Category;

namespace ECommerce.Application.Features.Categories.Queries.Get;

public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryResponse>>;

public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork
            .Repository<Category>()
            .GetByIdProjectAsync<CategoryResponse>(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.Id));

        return Result.Success(category);
    }
}