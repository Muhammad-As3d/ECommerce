using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Product : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Stock { get; set; }
    public int? ModelYear { get; set; }
    public double Price { get; set; }
    public bool IsActive { get; set; } = true;
    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];

    private Product() { }
    public static Product Create(string name, string description, int stock, int modelYear, double price, int categoryId, string createdById)
    {
        return new Product()
        {
            Name = name,
            Description = description,
            Stock = stock,
            ModelYear = modelYear,
            Price = price,
            CategoryId = categoryId,
            CreatedById = createdById,
            CreatedOn = DateTime.UtcNow,
        };
    }

    public void Update(string name, string description, int stock, int modelYear, double price, int categoryId, string updatedById)
    {
        Name = name;
        Description = description;
        Stock = stock;
        ModelYear = modelYear;
        Price = price;
        CategoryId = categoryId;
        UpdatedById = updatedById;
        UpdatedOn = DateTime.UtcNow;
    }
}

