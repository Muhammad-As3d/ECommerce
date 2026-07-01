namespace ECommerce.Application.Specifications.ProductSpecifications;

public class GetProductByIdSpecification : Specification<Product>
{
    public GetProductByIdSpecification(int categoryId, int productId)
    {
        Predicate = x =>
            x.Category.Id == categoryId &&
            x.Id == productId &&
            !x.IsDeleted;
    }
}
