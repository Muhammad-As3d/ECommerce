using ECommerce.Application.Contracts.Products;

namespace ECommerce.Application.Features.Products.Queries.GetProduct;

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork
            .Repository<Product>()
            .GetByIdProjectAsync<ProductResponse>(request.Id, cancellationToken);

        if (product is null)
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(request.Id));

        return Result.Success(product);
    }
}