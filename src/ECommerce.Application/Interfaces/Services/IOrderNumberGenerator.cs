namespace ECommerce.Application.Interfaces.Services;

public interface IOrderNumberGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}
