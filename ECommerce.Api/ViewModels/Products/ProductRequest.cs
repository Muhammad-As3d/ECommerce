using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Contracts.Products;

public record ProductRequest(
    string Name,
    string Description,
    int Stock,
    int ModelYear,
    double Price,
    List<IFormFile> Images
);