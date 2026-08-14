using System.Data;

namespace ECommerce.Infrastructure.Implementations.Services;

public sealed class SqlOrderNumberGenerator(ApplicationDbContext context) : IOrderNumberGenerator
{
    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        var shouldClose = connection.State != ConnectionState.Open;

        if (shouldClose)
            await connection.OpenAsync(cancellationToken);

        try
        {
            await using var command = connection.CreateCommand();

            command.CommandText =
                "SELECT NEXT VALUE FOR OrderNumberSequence";

            var result = await command.ExecuteScalarAsync(
                cancellationToken);

            var next = Convert.ToInt64(result);

            return $"ORD-{DateTime.UtcNow:yyyy}-{next:D6}";
        }
        finally
        {
            if (shouldClose)
                await connection.CloseAsync();
        }
    }
}