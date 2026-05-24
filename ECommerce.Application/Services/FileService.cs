using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Services;

public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
{
    private readonly string _imagePath = @$"{webHostEnvironment.WebRootPath}\Images";
    public async Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_imagePath, image.FileName);

        using var stream = File.Create(path);
        await image.CopyToAsync(stream, cancellationToken);

        return path;
    }
}
