using FluentValidation;

namespace ECommerce.Api.ViewModels;

public record CategoryRequest(string Name, string Description);


#region Validation

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(3, 500);
    }
}

#endregion
