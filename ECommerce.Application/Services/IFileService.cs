using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Services;

public interface IFileService
{
    Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default);
    Task<List<string>> UploadManyImageAsync(List<IFormFile> images, CancellationToken cancellationToken = default);
    Task DeleteImages(List<string> imagePaths);
}
