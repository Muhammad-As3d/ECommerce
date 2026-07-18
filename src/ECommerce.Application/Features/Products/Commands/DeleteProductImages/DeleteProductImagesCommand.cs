namespace ECommerce.Application.Features.Products.Commands.DeleteProductImages;

public record DeleteProductImagesCommand(
    Guid CategoryId,
    Guid ProductId
) : IRequest<Result>;