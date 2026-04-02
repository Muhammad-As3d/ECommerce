
namespace ECommerce.Infrastructure.Implementations;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private readonly Dictionary<Type, object> _repositories = [];
    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
            return (IGenericRepository<T>)repository;

        var newRepository = new GenericRepository<T>(_context);

        _repositories[type] = newRepository;

        return newRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
