using AutoMapper;

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
    public async Task<List<Product>> GetAllByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) =>
         await _context.Products.Where(p => ids.Contains(p.Id))
        .ToListAsync(cancellationToken);
}
