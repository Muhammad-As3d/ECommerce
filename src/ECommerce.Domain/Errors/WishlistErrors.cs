using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class WishlistErrors
{
    public static Error ProductIsOnWishlist =>
        Error.BadRequest("Wishlist.ProductIsOnWishlist", "you already add this product in your wishlist.");
    public static Error ItemNotFound =>
        Error.NotFound("Wishlist.ItemNotFound", "this item with Id are not found");
}
