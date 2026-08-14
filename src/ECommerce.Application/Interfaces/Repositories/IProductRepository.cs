namespace ECommerce.Application.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<bool> TryDecreaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task<List<ProductCheckoutInfo>> GetCheckoutInfoByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
