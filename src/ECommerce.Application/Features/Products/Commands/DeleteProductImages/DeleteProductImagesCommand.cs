namespace ECommerce.Application.Features.Products.Commands.DeleteProductImages;

public record DeleteProductImagesCommand(
    int CategoryId,
    int ProductId
) : IRequest<Result>;