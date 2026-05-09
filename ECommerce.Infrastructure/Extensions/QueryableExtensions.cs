namespace ECommerce.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> query, string[] includes) where T : class
    {
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query;
    }
}
