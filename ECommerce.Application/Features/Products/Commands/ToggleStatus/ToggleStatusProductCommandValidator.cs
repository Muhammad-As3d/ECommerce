using ECommerce.Application.Shared;

namespace ECommerce.Application.Features.Products.Commands.ToggleStatus;

public class ToggleStatusProductCommandValidator : AbstractValidator<ToggleStatusProductCommand>
{
    public ToggleStatusProductCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(c => c.categoryId)
            .SetValidator(new CategoryIdValidator(unitOfWork));
    }
}
