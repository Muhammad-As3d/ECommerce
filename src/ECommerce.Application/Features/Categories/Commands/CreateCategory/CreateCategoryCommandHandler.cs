using ECommerce.Application.Features.Categories.Commands.CreateCategory;

namespace ECommerce.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Category>();

        var isNameExists = await repo.AnyAsync(x => x.Name == request.Name, cancellationToken);

        if (isNameExists)
            return Result.Failure<int>(CategoryErrors.DuplicatedName);

        var category = Category.Create(request.Name, request.Description);

        await repo.AddAsync(category, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Id);
    }
}
