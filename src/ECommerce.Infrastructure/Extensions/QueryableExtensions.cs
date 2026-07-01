using ECommerce.Application.Abstractions.Pagination;

namespace ECommerce.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source,
        int pageNumber, int pageSize, CancellationToken cancellationToken = default) where T : class
    {
        var totalCount = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, pageNumber, totalCount, pageSize);
    }
}
