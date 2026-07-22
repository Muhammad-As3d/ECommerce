using ECommerce.Application.Features.Categories.Commands.UpdateCategory;

namespace ECommerce.Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Category>();

        var category = Category.CreateStub(request.Id);

        var changedProperties = category.Update(request.Name, request.Description);

        repo.PartialUpdate(category, changedProperties);

        var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return rowsAffected == 0
            ? Result.Failure(CategoryErrors.NotFound(request.Id))
            : Result.Success();
    }
}
