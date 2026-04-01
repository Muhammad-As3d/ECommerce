using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Category : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; } = [];
}
