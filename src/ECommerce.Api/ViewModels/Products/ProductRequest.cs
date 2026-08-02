namespace ECommerce.Api.ViewModels.Products;

public record ProductRequest(
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    decimal Price,
    List<IFormFile> Images
);

public record ProductUpdateRequest(
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    decimal Price
);


