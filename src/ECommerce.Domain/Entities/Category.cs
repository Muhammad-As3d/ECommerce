using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = [];

    private Category() { }
    public static Category CreateStub(Guid id) => new() { Id = id };

    public static Category Create(string name, string description) =>
         new()
         {
             Name = name,
             Description = description
         };

    public IReadOnlyCollection<string> Update(string name, string description)
    {
        Name = name;
        Description = description;

        return [nameof(Name), nameof(Description)];
    }
}