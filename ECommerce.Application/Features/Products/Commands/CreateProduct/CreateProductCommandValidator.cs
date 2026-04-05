using FluentValidation;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .Length(3, 255)
            .WithMessage("Product name must be at least 3 characters.");


        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(c => c.Price)
            .GreaterThan(0);

        RuleFor(c => c.Stock)
            .GreaterThanOrEqualTo(0);

        RuleFor(c => c.CategoryId)
            .LessThanOrEqualTo(0)
            .WithMessage("A valid category must be selected.");
    }
}
