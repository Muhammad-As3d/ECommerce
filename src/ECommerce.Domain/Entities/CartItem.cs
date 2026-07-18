using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class CartItem : AuditableEntity
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double UnitPriceSnapshot { get; set; }

    public Cart Cart { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
