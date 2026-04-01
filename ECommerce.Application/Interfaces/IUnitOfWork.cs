using ECommerce.Application.Interfaces.Repositories;

namespace ECommerce.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
