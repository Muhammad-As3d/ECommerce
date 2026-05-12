namespace ECommerce.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> query, Func<IQueryable<T>, IQueryable<T>> include) where T : class
    {
        if (include is not null)
            query = include(query);

        return query;
    }
}
