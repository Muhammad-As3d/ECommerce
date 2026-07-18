using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public sealed class ProductImage : AuditableEntity
{
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Product Product { get; set; } = default!;

    //public ProductImage() { }
}
