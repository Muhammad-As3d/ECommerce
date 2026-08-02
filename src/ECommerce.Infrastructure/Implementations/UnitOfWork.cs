using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Infrastructure.Implementations;

public class UnitOfWork(ApplicationDbContext context, IMapper mapper)
    : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private IDbContextTransaction? _transaction;

    private static readonly Dictionary<Type, Func<ApplicationDbContext, IMapper, object>> _specialized = new()
    {
        [typeof(Product)] = (ctx, mapper) => new ProductRepository(ctx, mapper)
    };

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
            return (IGenericRepository<T>)repository;

        var newRepository = _specialized.TryGetValue(type, out var factory)
            ? factory(_context, _mapper)
            : new GenericRepository<T>(_context, _mapper);

        _repositories[type] = newRepository;
        return (IGenericRepository<T>)newRepository;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
            await _transaction.DisposeAsync();

        await _context.DisposeAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
             await _context.SaveChangesAsync(cancellationToken);
}
