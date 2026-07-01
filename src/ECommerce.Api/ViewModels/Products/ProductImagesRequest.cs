namespace ECommerce.Api.ViewModels.Products;

public record ProductImagesRequest(
    List<IFormFile> Images
);
