using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Implementations.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().FindAsync(id, cancellationToken);

    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().AddAsync(entity, cancellationToken);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void ToggleStatusAsync(T entity) => _context.Set<T>().Remove(entity);
}
