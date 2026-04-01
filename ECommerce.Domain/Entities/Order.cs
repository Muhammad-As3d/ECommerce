using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order : AuditableEntity
{
    public int Id { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateOnly RequiredDate { get; set; }
    public DateOnly? ShippingDate { get; set; }
    public bool IsActive { get; set; } = true;
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
