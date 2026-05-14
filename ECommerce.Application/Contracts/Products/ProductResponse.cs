namespace ECommerce.Application.Contracts.Products;

public record ProductResponse(
    int Id,
    string Name,
    string Description,
    int Stock,
    int? ModelYear,
    double Price,
    int CategoryId
);
