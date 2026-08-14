using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Domain.Specifications;
using ECommerce.Infrastructure.Specifications;

namespace ECommerce.Infrastructure.Implementations.Repositories;

public class GenericRepository<T>(ApplicationDbContext context, IMapper mapper)
    : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbSet.ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> GetAllBySpecAsync(Specification<T> spec, CancellationToken cancellationToken = default) =>
        await SpecificationEvaluator
        .GetQuery(_dbSet, spec)
        .ToListAsync(cancellationToken);
    public async Task<IEnumerable<TProjection>> GetAllBySpecAsync<TProjection>(Specification<T> specification, Expression<Func<T, TProjection>> selector,
    CancellationToken cancellationToken = default) =>
         await SpecificationEvaluator
            .GetQuery(_dbSet.AsNoTracking(), specification)
            .Select(selector)
            .ToListAsync(cancellationToken);

    public async Task<T?> GetBySpecAsync(Specification<T> spec, CancellationToken cancellationToken = default) =>
        await SpecificationEvaluator
        .GetQuery(_dbSet, spec)
        .FirstOrDefaultAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default) =>
        await _dbSet.AddRangeAsync(entities, cancellationToken);

    public void PartialUpdate(T entity, IEnumerable<string> propertyNames)
    {
        var entry = _context.Entry(entity);

        if (entry.State is EntityState.Detached)
            entry = _dbSet.Attach(entity);

        foreach (var expr in propertyNames)
            entry.Property(expr).IsModified = true;
    }

    public async Task<int> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbSet
        .Where(c => c.Id == id)
        .ExecuteUpdateAsync(s => s
        .SetProperty(c => c.IsDeleted, x => !x.IsDeleted), cancellationToken);

    public async Task<int> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbSet
        .Where(c => c.Id == id)
        .ExecuteUpdateAsync(s => s
        .SetProperty(c => c.IsDeleted, x => true), cancellationToken);

    public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dbSet
        .Where(predicate)
        .ExecuteDeleteAsync(cancellationToken);
    public void DeleteRangeAsync(List<T> entities, CancellationToken cancellationToken = default) =>
         _dbSet.RemoveRange(entities);

    #region Checks

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dbSet.AnyAsync(predicate, cancellationToken);

    #endregion


    #region Projection & Pagination & Specification 
    public async Task<IEnumerable<TProjection>> GetAllProjectAsync<TProjection>(CancellationToken cancellationToken = default) where TProjection : class
        => await _dbSet
        .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

    public async Task<PaginatedList<TProjection>> GetAllPaginatedProjectAsync<TProjection>(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default) where TProjection : class =>
         await _dbSet
            .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

    public async Task<PaginatedList<TProjection>> GetAllPaginatedProjectAsync<TProjection>(Specification<T> spec,
         int pageNumber, int pageSize, CancellationToken cancellationToken = default) where TProjection : class =>
        await SpecificationEvaluator
            .GetQuery(_dbSet, spec)
            .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
    public async Task<TProjection?> GetByPredicateProjectAsync<TProjection>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProjection>> selector,
    CancellationToken cancellationToken = default) =>
         await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);

<<<<<<< Updated upstream
    public async Task<TProjection?> GetByIdProjectAsync<TProjection>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where TProjection : class =>
=======
    public async Task<TProjection?> GetByPredicateProjectAsync<TProjection>(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default) where TProjection : class =>
>>>>>>> Stashed changes
        await _dbSet
        .Where(predicate)
        .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    public async Task<TProjection?> GetBySpecProjectAsync<TProjection>(Specification<T> spec, CancellationToken cancellationToken = default) where TProjection : class =>
        await SpecificationEvaluator
        .GetQuery(_dbSet, spec)
        .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<TProjection>?> GetAllSpecProjectAsync<TProjection>(Specification<T> spec, CancellationToken cancellationToken = default) where TProjection : class =>
        await SpecificationEvaluator
        .GetQuery(_dbSet, spec)
        .ProjectTo<TProjection>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

    #endregion
}