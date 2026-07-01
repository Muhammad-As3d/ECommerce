using System.Linq.Expressions;

namespace ECommerce.Domain.Specifications;

public abstract class Specification<T> where T : class
{
    public Expression<Func<T, bool>>? Predicate { get; protected set; }
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> include) =>
        Includes.Add(include);

    protected void SortingBy(Expression<Func<T, object>> orderBy) =>
        OrderBy = orderBy;

    protected void SortingByDescending(Expression<Func<T, object>> orderByDescending) =>
        OrderByDescending = orderByDescending;
}