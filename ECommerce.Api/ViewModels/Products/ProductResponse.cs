namespace ECommerce.Api.ViewModels.Products;

public record ProductResponse(
    int Id,
    string Name,
    int ModelYear,
    double ListPrice,
    int CategoryId
    );
