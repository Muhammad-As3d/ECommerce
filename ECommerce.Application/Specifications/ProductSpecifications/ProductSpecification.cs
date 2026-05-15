using ECommerce.Application.Abstractions.Pagination;

namespace ECommerce.Application.Specifications.ProductSpecifications;

public class ProductSpecification : Specification<Product>
{
    public ProductSpecification(int? id, SpecFilters? spec)
    {
        Predicate = x =>
        (x.Id == id || !id.HasValue) &&
        (string.IsNullOrEmpty(spec!.SearchValue) || x.Name.ToLower() == spec.SearchValue.ToLower());
    }
}
