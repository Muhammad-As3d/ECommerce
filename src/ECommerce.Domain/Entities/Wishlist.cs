using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Wishlist : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;

    public ICollection<WishlistItem> Items { get; set; } = [];

    private Wishlist() { }

    public static Wishlist Create(string userId) =>
        new() { UserId = userId };
}