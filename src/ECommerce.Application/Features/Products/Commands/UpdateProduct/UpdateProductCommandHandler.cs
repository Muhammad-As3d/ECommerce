using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
    : IRequestHandler<UpdateProductCommand, Result>
{
    private const string CacheKeyPrefix = "product";

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeyPrefix}:{request.CategoryId}:{request.Id}";

        await distributedCache.RemoveAsync(cacheKey, cancellationToken);

        var repo = unitOfWork.Repository<Product>();

        var categoryIsExist = await unitOfWork.Repository<Category>().AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryIsExist)
            return Result.Failure<Guid>(CategoryErrors.NotFound(request.CategoryId));

        var productIsExist = await repo.AnyAsync(x => x.Id == request.Id && x.CategoryId == request.CategoryId, cancellationToken);

        if (!productIsExist)
            return Result.Failure(ProductErrors.NotFound(request.Id));

        var product = Product.CreateStub(request.Id);
        var changedProperties = product.Update(request.Name, request.Description, request.Stock, request.ModelYear, request.Price);
        repo.PartialUpdate(product, changedProperties);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
