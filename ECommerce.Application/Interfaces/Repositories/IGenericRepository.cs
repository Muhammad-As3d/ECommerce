using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ECommerce.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllBySpecAsync(Specification<T> spec, CancellationToken cancellationToken = default);
    Task<T?> GetBySpecAsync(Specification<T> spec, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);
    void PartialUpdate(T entity, params Expression<Func<T, object>>[] properties);
    Task<int> ToggleStatusAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default);
    void DeleteRangeAsync(List<T> entities, CancellationToken cancellationToken = default);

    #region Checks
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Projection & Pagination & Specification 

    Task<IEnumerable<TProjection>> GetAllProjectAsync<TProjection>(CancellationToken cancellationToken = default) where TProjection : class;
    Task<TProjection?> GetByIdProjectAsync<TProjection>(int id, CancellationToken cancellationToken = default) where TProjection : class;
    Task<PaginatedList<TProjection>> GetAllPaginatedProjectAsync<TProjection>(int pageNumber, int pageSize,
    CancellationToken cancellationToken = default) where TProjection : class;
    Task<PaginatedList<TProjection>> GetAllPaginatedProjectAsync<TProjection>(Specification<T> spec,
        int pageNumber, int pageSize, CancellationToken cancellationToken = default) where TProjection : class;
    Task<TProjection?> GetBySpecProjectAsync<TProjection>(Specification<T> spec, CancellationToken cancellationToken = default) where TProjection : class;
    Task<List<TProjection>?> GetAllSpecProjectAsync<TProjection>(Specification<T> spec, CancellationToken cancellationToken = default) where TProjection : class;

    #endregion
}
