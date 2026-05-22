using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Interfaces.Services;

public interface IFileService
{
    Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default);
}
