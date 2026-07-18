using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Cart : AuditableEntity
{
    public string UserId { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; } = [];
}
