using ECommerce.Application.Abstractions.Constants;
using ECommerce.Application.Shared;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IUnitOfWork unitOfWork)
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
            .GreaterThan(0);

        RuleFor(c => c.categoryId)
            .SetValidator(new CategoryIdValidator(unitOfWork));

        RuleFor(c => c.Images)
            .Must((request, images) =>
            {
                return images.All(file =>
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    return FileSettings.AllowedImagesExtensions.Contains(extension);
                });
            })
            .WithMessage("File extension is not allowed, Allowed extension is (.jpg,.jpeg,.png)")
            .When(x => x.Images is not null);
    }
}