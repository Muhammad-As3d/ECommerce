namespace ECommerce.Application.Specifications.ProductSpecifications;

public class GetProductByIdSpecification : Specification<Product>
{
    public GetProductByIdSpecification(Guid categoryId, Guid productId)
    {
        Predicate = x =>
            x.Category.Id == categoryId &&
            x.Id == productId &&
            !x.IsDeleted;
    }
}