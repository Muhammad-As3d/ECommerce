using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = [];

    public Category() { }
    public static Category Create(string name, string description) =>
         new()
         {
             Name = name,
             Description = description
         };

}
