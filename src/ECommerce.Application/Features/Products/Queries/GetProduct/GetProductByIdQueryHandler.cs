using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Specifications.ProductSpecifications;

namespace ECommerce.Application.Features.Products.Queries.GetProduct;

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
    : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    private const string CacheKeyPrefix = "product";
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeyPrefix}:{request.CategoryId}:{request.Id}";

        var cachedProduct = await cacheService.GetAsync<ProductResponse>(cacheKey, cancellationToken);

        if (cachedProduct is not null)
            return Result.Success(cachedProduct);

        var categoryIsExists = await unitOfWork
                .Repository<Category>()
                .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryIsExists)
            return Result.Failure<ProductResponse>(CategoryErrors.NotFound(request.CategoryId));

        var spec = new GetProductByIdSpecification(request.CategoryId, request.Id);

        var product = await unitOfWork
                .Repository<Product>()
                .GetBySpecProjectAsync<ProductResponse>(spec, cancellationToken);

        if (product is null)
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(request.Id));

        await cacheService.SetAsync(cacheKey, product, cancellationToken: cancellationToken);

        return Result.Success(product);
    }
}