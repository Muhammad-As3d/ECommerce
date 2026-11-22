using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class WishlistItem : BaseEntity
{
    public Guid WishlistId { get; set; }
    public Guid ProductId { get; set; }

    public Wishlist Wishlist { get; set; } = null!;
    public Product Product { get; set; } = null!;

    private WishlistItem() { }

    public static WishlistItem Create(Guid wishlistId, Guid productId) =>
        new()
        {
            WishlistId = wishlistId,
            ProductId = productId
        };
}