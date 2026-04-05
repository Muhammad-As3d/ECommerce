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

    private Product() { }
    public static Product Create(string name, int modelYear, double listPrice, int categoryId, string createdById)
    {
        return new Product()
        {
            Name = name,
            ModelYear = modelYear,
            ListPrice = listPrice,
            CategoryId = categoryId,
            CreatedById = createdById,
            CreatedOn = DateTime.UtcNow,
        };
    }

    public void Update(string name, int modelYear, double listPrice, int categoryId, string updatedById)
    {
        Name = name;
        ModelYear = modelYear;
        ListPrice = listPrice;
        CategoryId = categoryId;
        UpdatedById = updatedById;
        UpdatedOn = DateTime.UtcNow;
    }
}

