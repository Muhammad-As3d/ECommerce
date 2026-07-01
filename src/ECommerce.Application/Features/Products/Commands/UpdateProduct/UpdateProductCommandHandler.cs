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

        var exists = await repo.AnyAsync(x => x.Id == request.Id, cancellationToken);

        if (!exists)
            return Result.Failure(ProductErrors.NotFound(request.Id));

        var product = Product.CreateStub(request.Id);

        product.Update(request.Name, request.Description, request.Stock, request.ModelYear, request.Price);

        repo.PartialUpdate(product, x => x.Name, x => x.Description, x => x.Stock, x => x.ModelYear!, x => x.Price);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
