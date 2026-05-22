namespace ECommerce.Application.Features.Categories.Commands.ToggleStatus;

public class ToggleStatusCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ToggleStatusCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ToggleStatusCategoryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Category>();

        var isCategoryHasProducts = await repo.AnyAsync(x => x.Id == request.Id && x.Products.Any(), cancellationToken);

        if (isCategoryHasProducts)
            return Result.Failure(CategoryErrors.CategoryHasProducts);

        var affectedRows = await repo.ToggleStatusAsync(request.Id, cancellationToken);

        if (affectedRows == 0)
            return Result.Failure(CategoryErrors.NotFound(request.Id));

        return Result.Success();
    }
}
