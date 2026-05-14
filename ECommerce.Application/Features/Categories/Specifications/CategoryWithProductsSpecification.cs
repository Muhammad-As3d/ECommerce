using ECommerce.Domain.Specifications;

namespace ECommerce.Application.Features.Categories.Specifications;

public class CategoryWithProductsSpecification : Specification<Category>
{
    public CategoryWithProductsSpecification(int id)
    {
        Predicate = x => x.Id == id;
        //AddInclude(q => q.Products);
    }
}
