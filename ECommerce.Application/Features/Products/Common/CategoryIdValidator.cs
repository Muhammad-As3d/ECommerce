namespace ECommerce.Application.Features.Products.Common;

public class CategoryIdValidator : AbstractValidator<int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryIdValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x)
            .GreaterThan(0)
            .MustAsync(CategoryExistsAsync)
            .WithMessage("Category not found");
    }

    private async Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
    {
        return await _unitOfWork
            .Repository<Category>()
            .AnyAsync(c => c.Id == categoryId, cancellationToken);
    }
}
