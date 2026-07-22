using AutoMapper;

namespace ECommerce.Infrastructure.Implementations;

public class UnitOfWork(ApplicationDbContext context, IMapper mapper)
    : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
            return (IGenericRepository<T>)repository;

        var newRepository = new GenericRepository<T>(_context, _mapper);

        _repositories[type] = newRepository;

        return newRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}
