namespace ECommerce.Application.Specifications.CategorySpecifications;

public class CategoryWithProductsSpecification : Specification<Category>
{
    public CategoryWithProductsSpecification(Guid id)
    {
        Predicate = x => x.Id == id;
        AddInclude(q => q.Products);
    }
}
