using ECommerce.Application.Abstractions.Pagination;
using System.Linq.Expressions;

namespace ECommerce.Application.Specifications.CategorySpecifications;

public class CategorySpecification : Specification<Category>
{
    public CategorySpecification(SpecificationRequest spec)
    {
        Predicate = x =>
        (string.IsNullOrEmpty(spec.SearchValue) || x.Name.Contains(spec.SearchValue.ToLower()));

        ApplySorting(spec);
    }

    private void ApplySorting(SpecificationRequest spec)
    {
        Action<Expression<Func<Category, object>>> sort = spec.IsDescending ? SortingByDescending : SortingBy;

        switch (spec.SortColumn?.ToLower())
        {
            case "name":
                sort(x => x.Name);
                break;

            case "description":
                sort(x => x.Description);
                break;

            default:
                sort(x => x.Id);
                break;
        }
    }
}
