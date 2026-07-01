namespace ECommerce.Application.Specifications.ProductSpecifications;

public class ProductImageSpecification : Specification<ProductImage>
{
    public ProductImageSpecification(int productId)
    {
        Predicate = x => x.ProductId == productId && !x.IsDeleted;
    }
}
