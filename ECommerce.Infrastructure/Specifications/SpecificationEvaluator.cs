using ECommerce.Domain.Specifications;
namespace ECommerce.Infrastructure.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, Specification<T> spec) where T : class
    {
        var query = inputQuery.AsQueryable();

        if (spec.Predicate is not null)
            query = query.Where(spec.Predicate);

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}
