using ECommerce.Application.Abstractions.Constants;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Products.Commands.CreateProductImage;

public record CreateProductImagesCommand(
    int CategoryId,
    int ProductId,
    List<IFormFile> Images
) : IRequest<Result>;

#region Validation
public class CreateProductImagesCommandValidator : AbstractValidator<CreateProductImagesCommand>
{
    public CreateProductImagesCommandValidator()
    {

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
#endregion