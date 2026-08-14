using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public sealed class Product : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public int ReservedStock { get; private set; }
    public int? ModelYear { get; private set; }

    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }

    public byte[] RowVersion { get; private set; } = [];

    public int AvailableStock => Stock - ReservedStock;

    public Category Category { get; private set; } = null!;
    public ICollection<ProductImage> ProductImages { get; private set; } = [];

    private Product() { }

    public static Product CreateStub(Guid id) => new() { Id = id };

    public static Product Create(string name, string description, int stock, int modelYear, decimal price, Guid categoryId) =>
        new()
        {
            Name = name,
            Slug = name.Trim().ToLowerInvariant().Replace(' ', '-'),
            Sku = $"SKU-{Guid.NewGuid():N}"[..16].ToUpperInvariant(),
            Description = description,
            Stock = stock,
            ModelYear = modelYear,
            Price = price,
            CategoryId = categoryId,
            IsActive = true
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

    public void AddImages(IEnumerable<string> imageUrls)
    {
        foreach (var imageUrl in imageUrls)
            ProductImages.Add(new ProductImage { ImageUrl = imageUrl });
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0 || AvailableStock < quantity)
            throw new InvalidOperationException("Insufficient stock.");

        ReservedStock += quantity;
    }

    public void ConfirmReservedStock(int quantity)
    {
        if (quantity <= 0 || ReservedStock < quantity)
            throw new InvalidOperationException("Invalid reserved quantity.");

        ReservedStock -= quantity;
        Stock -= quantity;
    }

    public void ReleaseReservedStock(int quantity)
    {
        if (quantity <= 0 || ReservedStock < quantity)
            throw new InvalidOperationException("Invalid reserved quantity.");

        ReservedStock -= quantity;
    }
}
