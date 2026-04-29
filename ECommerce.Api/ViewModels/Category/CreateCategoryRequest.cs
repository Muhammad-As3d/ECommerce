using FluentValidation;

namespace ECommerce.Api.ViewModels.Category;

public record CreateCategoryRequest(string Name);

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .Length(3, 266);
    }
}