using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public sealed class ProductImage : AuditableEntity
{
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public Product Product { get; set; } = default!;

    public ProductImage() { }
}
