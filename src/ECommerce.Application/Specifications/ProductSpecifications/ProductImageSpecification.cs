namespace ECommerce.Application.Specifications.ProductSpecifications;

public class ProductImageSpecification : Specification<ProductImage>
{
    public ProductImageSpecification(Guid productId)
    {
        Predicate = x => x.ProductId == productId && !x.IsDeleted;
    }
}
