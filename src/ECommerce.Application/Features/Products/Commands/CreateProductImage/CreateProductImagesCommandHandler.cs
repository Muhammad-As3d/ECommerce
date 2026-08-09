using ECommerce.Application.Contracts.Products;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.Application.Features.Products.Commands.CreateProductImage;

internal class CreateProductImagesCommandHandler(IUnitOfWork unitOfWork, IFileService fileService, IDistributedCache distributedCache)
    : IRequestHandler<CreateProductImagesCommand, Result>
{
    private const string CacheKeyPrefix = "product";

    public async Task<Result> Handle(CreateProductImagesCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeyPrefix}:{request.CategoryId}:{request.ProductId}";

        await distributedCache.RemoveAsync(cacheKey, cancellationToken);

        var categoryIsExists = await unitOfWork
            .Repository<Category>()
            .AnyAsync(x => x.Id == request.CategoryId && !x.IsDeleted, cancellationToken);

        if (!categoryIsExists)
            return Result.Failure<ProductResponse>(CategoryErrors.NotFound(request.CategoryId));

        var repo = unitOfWork.Repository<Product>();

        var isExists = await repo.AnyAsync(x => x.Id == request.ProductId && x.CategoryId == request.CategoryId && !x.IsDeleted, cancellationToken);

        if (!isExists)
            return Result.Failure(ProductErrors.NotFound(request.ProductId));

        var imagesUrls = await fileService.UploadManyImageAsync(request.Images, cancellationToken);

        var productImages = imagesUrls.Select(url => new ProductImage
        {
            ImageUrl = url,
            ProductId = request.ProductId
        }).ToList();

        await unitOfWork.Repository<ProductImage>().AddRangeAsync(productImages, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
