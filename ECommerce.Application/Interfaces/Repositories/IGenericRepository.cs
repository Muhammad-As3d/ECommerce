using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> includes = null!, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> includes = null!, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    Task<int> ToggleStatusAsync(int id, CancellationToken cancellationToken = default);

    #region Checks
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Projection
    Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(CancellationToken cancellationToken = default) where TProjection : class;
    Task<TProjection?> GetByIdProjectionAsync<TProjection>(int id, CancellationToken cancellationToken = default) where TProjection : class;
    #endregion
}
