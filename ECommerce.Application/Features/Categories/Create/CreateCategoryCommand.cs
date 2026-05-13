using FluentValidation;

namespace ECommerce.Application.Features.Categories.Create;

public record CreateCategoryCommand(
    string Name,
    string Description
) : IRequest<Result>;


# region Validation 

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
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
