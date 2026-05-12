using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ECommerce.Infrastructure.Implementations.Repositories;

public class GenericRepository<T>(ApplicationDbContext context, IMapper mapper) : IGenericRepository<T> where T : BaseEntity
{
    private readonly IMapper _mapper = mapper;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbSet.ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> includes = null!, CancellationToken cancellationToken = default) =>
        await _dbSet.ApplyIncludes(includes).ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbSet.FindAsync(id, cancellationToken);

    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> includes = null!, CancellationToken cancellationToken = default) =>
        await _dbSet.Where(predicate).ApplyIncludes(includes).FirstOrDefaultAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity) => _dbSet.Update(entity);

    public async Task<int> ToggleStatusAsync(int id, CancellationToken cancellationToken = default) =>
        await _dbSet
        .Where(c => c.Id == id)
        .ExecuteUpdateAsync(s => s
        .SetProperty(c => c.IsDeleted, x => !x.IsDeleted), cancellationToken);

    #region Checks

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dbSet.AnyAsync(predicate, cancellationToken);

    #endregion


    #region Projection
    public async Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(CancellationToken cancellationToken = default) where TProjection : class
        => await _dbSet.AsNoTracking()
        .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

    public async Task<TProjection?> GetByIdProjectionAsync<TProjection>(int id, CancellationToken cancellationToken = default) where TProjection : class =>
        await _dbSet.Where(x => x.Id == id).ProjectTo<TProjection>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

    #endregion
}
