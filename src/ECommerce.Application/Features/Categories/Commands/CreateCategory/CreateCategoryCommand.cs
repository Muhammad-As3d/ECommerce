namespace ECommerce.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string Description
) : IRequest<Result<Guid>>;


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
