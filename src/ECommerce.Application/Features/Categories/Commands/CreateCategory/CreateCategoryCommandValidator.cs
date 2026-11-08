using Microsoft.Extensions.Localization;

namespace ECommerce.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode("Validation.Required")
            .WithMessage(_ => localizer["Validation.Required", localizer["Fields.Name"].Value].Value)
            .Length(3, 100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(3, 500);
    }
}

