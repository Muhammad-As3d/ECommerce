using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class CartErrors
{
    public static Error QuantityNotAvailable =>
        Error.BadRequest("Cart.QuantityNotAvailable", "Sorry we not have all this quantity.");
    public static Error ProductIsOnCart =>
        Error.BadRequest("Cart.ProductIsOnCart", "you already add this product in your cart.");
    public static Error CartItemsNotFound =>
        Error.NotFound("Cart.CartItemsNotFound", "You didn't add any items into basket");
}