using ECommerce.Application.Contracts.Products;

namespace ECommerce.Application.Contracts.Category;

public record CategoryProductsResponse(
    int Id,
    string Name,
    string Description,
    IEnumerable<ProductResponse> Products
);
