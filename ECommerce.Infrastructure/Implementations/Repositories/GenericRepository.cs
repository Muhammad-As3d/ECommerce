using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Implementations.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().AsNoTracking().ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate) =>
        await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);

    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}
