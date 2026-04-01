using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Product : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ModelYear { get; set; }
    public double ListPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;

    public ICollection<OrderItem> OrderItems { get; set; } = [];
}

