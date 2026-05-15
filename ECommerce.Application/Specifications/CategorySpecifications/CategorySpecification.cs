using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Domain.Specifications;

namespace ECommerce.Application.Specifications.CategorySpecifications;

public class CategorySpecification : Specification<Category>
{
    public CategorySpecification(SpecFilters spec)
    {
        Predicate = x =>
        (string.IsNullOrEmpty(spec.SearchValue) || x.Name.Contains(spec.SearchValue.ToLower()));

        var columnName = spec.SortColumn?.ToLower();

        switch (columnName)
        {
            case "name":
                if (spec.IsDescending)
                    SortingByDescending(x => x.Name);
                else
                    SortingBy(x => x.Name);
                break;

            case "description":
                if (spec.IsDescending)
                    SortingByDescending(x => x.Description);
                else
                    SortingBy(x => x.Description);
                break;

            default:
                if (spec.IsDescending)
                    SortingByDescending(x => x.Id);
                else
                    SortingBy(x => x.Id);
                break;
        }
    }
}
