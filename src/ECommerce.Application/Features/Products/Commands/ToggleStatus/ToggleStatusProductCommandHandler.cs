using ECommerce.Application.Contracts.Products;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.Application.Features.Products.Commands.ToggleStatus;

internal class ToggleStatusProductCommandHandler(IUnitOfWork unitOfWork, IDistributedCache distributedCache)
    : IRequestHandler<ToggleStatusProductCommand, Result>
{
    private const string CacheKeyPrefix = "product";

    public async Task<Result> Handle(ToggleStatusProductCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeyPrefix}:{request.CategoryId}:{request.Id}";

        await distributedCache.RemoveAsync(cacheKey, cancellationToken);

        var categoryIsExists = await unitOfWork.Repository<Category>().AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryIsExists)
            return Result.Failure<ProductResponse>(CategoryErrors.NotFound(request.CategoryId));

        var affectedRows = await unitOfWork
            .Repository<Product>()
            .ToggleStatusAsync(request.Id, cancellationToken);

        return affectedRows == 0
            ? Result.Failure(ProductErrors.NotFound(request.Id))
            : Result.Success();
    }
}
