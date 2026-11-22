namespace ECommerce.Application.Specifications.WishlistSpecification;

public class WishlistItemsSpecification : Specification<WishlistItem>
{
    public WishlistItemsSpecification(Guid wishlistId)
    {
        Predicate = x => x.WishlistId == wishlistId;
    }
}

public class WishlistSpecification : Specification<Wishlist>
{
    public WishlistSpecification(string UserId)
    {
        Predicate = x => x.UserId == UserId;
    }
}
