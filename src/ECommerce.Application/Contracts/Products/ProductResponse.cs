namespace ECommerce.Application.Contracts.Products;

public record ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Stock { get; init; }
    public int? ModelYear { get; init; }
    public double Price { get; init; }
    public List<string> ImageURLs { get; init; } = [];
}