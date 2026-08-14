namespace ECommerce.Application.Contracts.Products;

public sealed record ProductCheckoutInfo(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    int Stock,
    bool IsDeleted,
    bool IsActive);