namespace ECommerce.Application.Contracts.Products;

public record GetProductDto(
    int Id,
    string Name,
    int ModelYear,
    double ListPrice,
    int CategoryId
);
