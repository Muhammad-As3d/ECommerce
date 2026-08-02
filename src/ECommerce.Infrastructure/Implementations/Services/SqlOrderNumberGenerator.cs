namespace ECommerce.Infrastructure.Implementations.Services;

public class SqlOrderNumberGenerator(ApplicationDbContext context) : IOrderNumberGenerator
{
    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var next = await context.Orders.CountAsync(cancellationToken) + 1;

        return $"ORD-{DateTime.UtcNow:yyyy}-{next:D6}";
    }
}
