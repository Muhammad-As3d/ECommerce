using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Product : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Stock { get; set; }
    public int? ModelYear { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }

    public ICollection<ProductImage> ProductImages { get; set; } = [];
    public Category Category { get; set; } = default!;

    private Product() { }

    public static Product CreateStub(Guid id) => new() { Id = id };

    public static Product Create(string name, string description, int stock, int modelYear, decimal price, Guid categoryId)
        =>
         new()
         {
             Name = name,
             Description = description,
             Stock = stock,
             ModelYear = modelYear,
             Price = price,
             CategoryId = categoryId
         };

    public IReadOnlyCollection<string> Update(string name, string description, int stock, int modelYear, decimal price)
    {
        Name = name;
        Description = description;
        Stock = stock;
        ModelYear = modelYear;
        Price = price;

        return [nameof(Name), nameof(Description), nameof(Stock), nameof(ModelYear), nameof(Price)];
    }
    public IReadOnlyCollection<string> DecreaseStock(int amount)
    {
        Stock = Stock - amount;

        return [nameof(Stock)];
    }

    public void AddImages(List<string> imageUrls)
    {
        List<ProductImage> uploadedImages = [];

        foreach (var image in imageUrls)
        {
            uploadedImages.Add(new ProductImage { ImageUrl = image });
        }

        foreach (ProductImage image in uploadedImages)
            ProductImages.Add(image);
    }
}