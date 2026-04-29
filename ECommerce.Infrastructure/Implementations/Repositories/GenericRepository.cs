namespace ECommerce.Infrastructure.Implementations.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context = context;

    public IQueryable<T> GetQueryable() => _context.Set<T>();
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Set<T>().ToListAsync(cancellationToken);
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().FindAsync(id, cancellationToken);

    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().AddAsync(entity, cancellationToken);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public async Task<int> ToggleStatusAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().Where(c => c.Id == id).ExecuteUpdateAsync(s => s
        .SetProperty(c => c.IsDeleted, x => !x.IsDeleted), cancellationToken);
}
