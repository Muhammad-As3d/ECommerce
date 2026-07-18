using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Review : AuditableEntity
{
    public Guid ProductId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid OrderItemId { get; set; }

    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }
    public bool IsApproved { get; set; }

    public Product Product { get; set; } = null!;
    public OrderItem OrderItem { get; set; } = null!;
}
