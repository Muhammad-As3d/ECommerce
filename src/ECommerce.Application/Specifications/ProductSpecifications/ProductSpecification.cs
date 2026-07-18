using ECommerce.Application.Abstractions.Pagination;
using System.Linq.Expressions;

namespace ECommerce.Application.Specifications.ProductSpecifications;

public class ProductSpecification : Specification<Product>
{
    public ProductSpecification(Guid categoryId, SpecificationRequest spec, Guid? id = default)
    {
        Predicate = x =>
        x.Category.Id == categoryId &&
        (x.Id == id || !id.HasValue) && !x.IsDeleted &&
        (string.IsNullOrEmpty(spec.SearchValue) || x.Name.ToLower().Contains(spec.SearchValue.ToLower()));

        ApplySorting(spec);
    }

    private void ApplySorting(SpecificationRequest spec)
    {
        Action<Expression<Func<Product, object>>> sort = spec.IsDescending ? SortingByDescending : SortingBy;

        switch (spec.SortColumn?.ToLower())
        {
            case "name":
                sort(x => x.Name);
                break;

            case "description":
                sort(x => x.Description);
                break;

            case "price":
                sort(x => x.Price);
                break;

            default:
                sort(x => x.Id);
                break;
        }
    }
}
