using ECommerce.Application.Features.Categories.Commands.UpdateCategory;

namespace ECommerce.Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Category>();

        var exists = await repo.AnyAsync(x => x.Id == request.Id, cancellationToken);

        if (!exists)
            return Result.Failure(CategoryErrors.NotFound(request.Id));

        var category = Category.CreateStub(request.Id);

        category.Update(request.Name, request.Description);

        repo.PartialUpdate(category, x => x.Name, x => x.Description);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
