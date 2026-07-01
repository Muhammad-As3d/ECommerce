using ECommerce.Domain.Specifications;

namespace ECommerce.Application.Specifications.CategorySpecifications;

public class CategoryWithProductsSpecification : Specification<Category>
{
    public CategoryWithProductsSpecification(int id)
    {
        Predicate = x => x.Id == id;
        AddInclude(q => q.Products);
    }
}
