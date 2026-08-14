using AutoMapper;
using ECommerce.Application.Contracts.Products;

namespace ECommerce.Infrastructure.Implementations.Repositories;

internal class ProductRepository(ApplicationDbContext context, IMapper mapper)
    : GenericRepository<Product>(context, mapper), IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> TryDecreaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var affected = await _context.Products
            .Where(p => p.Id == productId && p.Stock >= quantity)
            .ExecuteUpdateAsync(p => p.SetProperty(p => p.Stock, p => p.Stock - quantity), cancellationToken);

        return affected > 0;
    }
    public async Task<List<ProductCheckoutInfo>> GetCheckoutInfoByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) =>
         await _context.Products.Where(p => ids.Contains(p.Id))
        .Select(s => new ProductCheckoutInfo(
            s.Id,
            s.Name,
            s.Sku,
            s.Price,
            s.Stock,
            s.IsDeleted,
            s.IsActive))
        .ToListAsync(cancellationToken);
}
