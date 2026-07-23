using ECommerce.Application.Contracts.ProductImages;
using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Specifications.ProductSpecifications;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.Application.Features.Products.Commands.DeleteProductImages;

public class DeleteProductImagesCommandHandler(IUnitOfWork unitOfWork, IFileService fileService, IDistributedCache distributedCache)
    : IRequestHandler<DeleteProductImagesCommand, Result>
{
    private const string CacheKeyPrefix = "product";

    public async Task<Result> Handle(DeleteProductImagesCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeyPrefix}:{request.CategoryId}:{request.ProductId}";

        await distributedCache.RemoveAsync(cacheKey, cancellationToken);

        var categoryIsExists = await unitOfWork
            .Repository<Category>()
            .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryIsExists)
            return Result.Failure<ProductResponse>(CategoryErrors.NotFound(request.CategoryId));

        var repo = unitOfWork.Repository<ProductImage>();

        var spec = new ProductImageSpecification(request.ProductId);

        var images = await repo
            .GetAllSpecProjectAsync<ProductImageResponse>(spec, cancellationToken);

        if (images is null || images.Count == 0)
            return Result.Failure(ProductErrors.NotfoundProductImages);

        foreach (var image in images)
        {
            await repo.DeleteAsync(x => x.Id == image.Id, cancellationToken);
        }

        var urls = images.Select(x => x.Url).ToList();
        await fileService.DeleteImages(urls);

        return Result.Success();
    }
}