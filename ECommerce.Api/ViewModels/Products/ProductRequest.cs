namespace ECommerce.Api.ViewModels.Products;

public record ProductRequest(string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price,
    int CategoryId
    //string CreatedById);
    );
