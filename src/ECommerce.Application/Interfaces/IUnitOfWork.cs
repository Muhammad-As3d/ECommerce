using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities.Common;

namespace ECommerce.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
