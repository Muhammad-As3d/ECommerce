using FluentValidation;

namespace ECommerce.Application.Features.Categories.Update;

public record UpdateCategoryCommand(
    int Id,
    string Name,
    string Description
) 
    : IRequest<Result>;



#region Validation

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
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
