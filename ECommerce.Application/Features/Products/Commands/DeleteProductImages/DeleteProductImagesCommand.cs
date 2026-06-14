using ECommerce.Application.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Features.Products.Commands.DeleteProductImages;

public record DeleteProductImagesCommand(
    [FromRoute] int categoryId,
    int ProductId
) : IRequest<Result>;


public class DeleteProductImagesCommandValidator : AbstractValidator<DeleteProductImagesCommand>
{
    public DeleteProductImagesCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(c => c.categoryId)
            .SetValidator(new CategoryIdValidator(unitOfWork));
    }
}
