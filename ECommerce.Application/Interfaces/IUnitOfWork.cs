using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities.Common;

namespace ECommerce.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
