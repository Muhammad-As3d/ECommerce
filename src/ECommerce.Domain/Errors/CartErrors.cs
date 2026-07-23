using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class CartErrors
{
    public static Error QuantityNotAvailable =>
        Error.Conflict("Cart.QuantityNotAvailable", "Sorry we not have all this quantity.");
    public static Error ProductIsOnCart =>
        Error.BadRequest("Cart.ProductIsOnCart", "you already add this product in your cart.");
    public static Error CartItemsNotFound =>
        Error.NotFound("Cart.CartItemsNotFound", "You didn't add any items into basket");
    public static Error ItemsNotFound =>
        Error.NotFound("Cart.ItemsNotFound", "this item with Id are not found");
    public static Error InsufficientStock =>
        Error.Conflict("Cart.InsufficientStock", "Sorry we not have all this quantity.");
}