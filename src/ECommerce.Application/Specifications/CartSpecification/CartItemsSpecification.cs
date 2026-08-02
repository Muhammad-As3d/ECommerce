namespace ECommerce.Application.Specifications.CartSpecification;

public class CartItemsSpecification : Specification<CartItem>
{
    public CartItemsSpecification(Guid cartId)
    {
        Predicate = x => x.CartId == cartId;
    }
}

public class OrderItemsSpecification : Specification<CartItem>
{
    public OrderItemsSpecification(string UserId)
    {
        Predicate = x => x.Cart.UserId == UserId;
    }
}

public class CartSpecification : Specification<Cart>
{
    public CartSpecification(string UserId)
    {
        Predicate = x => x.UserId == UserId;
    }
}
