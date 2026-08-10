namespace ECommerce.Application.Specifications.OrderSpecifications;

internal class OrderSpecification : Specification<Order>
{
    public OrderSpecification(string UserId)
    {
        Predicate = x => x.UserId == UserId;

        SortingBy(x => x.CreatedOn);
    }
}
