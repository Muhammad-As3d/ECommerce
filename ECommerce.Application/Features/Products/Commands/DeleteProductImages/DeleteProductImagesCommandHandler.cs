using ECommerce.Application.Contracts.ProductImages;
using ECommerce.Application.Services;
using ECommerce.Application.Specifications.ProductSpecifications;

namespace ECommerce.Application.Features.Products.Commands.DeleteProductImages;

public class DeleteProductImagesCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    : IRequestHandler<DeleteProductImagesCommand, Result>
{
    public async Task<Result> Handle(DeleteProductImagesCommand request, CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<ProductImage>();

        var spec = new ProductImageSpecification(request.ProductId);

        var images = await repo
            .GetAllSpecProjectAsync<ProductImageResponse>(spec, cancellationToken);

        if (images is null || !images.Any())
            return Result.Failure(ProductErrors.NotFound(request.ProductId));

        foreach (var image in images)
        {
            await repo.DeleteAsync(image.Id, cancellationToken);
        }

        var urls = images.Select(x => x.Url).ToList();
        await fileService.DeleteImages(urls);

        return Result.Success();
    }
}