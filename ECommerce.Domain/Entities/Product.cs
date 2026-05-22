using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Product : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Stock { get; set; }
    public int? ModelYear { get; set; }
    public double Price { get; set; }
    public int CategoryId { get; set; }

    public ICollection<ProductImage> ProductImages { get; set; } = [];
    public Category Category { get; set; } = default!;

    private Product() { }

    public static Product Create(string name, string description, int stock, int modelYear, double price, int categoryId) =>
         new()
         {
             Name = name,
             Description = description,
             Stock = stock,
             ModelYear = modelYear,
             Price = price,
             CategoryId = categoryId
         };

    public void Update(string name, string description, int stock, int modelYear, double price, int categoryId)
    {
        Name = name;
        Description = description;
        Stock = stock;
        ModelYear = modelYear;
        Price = price;
        CategoryId = categoryId;
    }

    public void AddImages(IEnumerable<ProductImage> images)
    {
        foreach (ProductImage image in images)
            ProductImages.Add(image);
    }
}

