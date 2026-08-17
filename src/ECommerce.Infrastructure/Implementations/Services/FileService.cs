using Microsoft.AspNetCore.Hosting;

namespace ECommerce.Infrastructure.Implementations.Services;

public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
{
    private readonly string _imagePath = Path.Combine(webHostEnvironment.WebRootPath, "Images");

    public async Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_imagePath);

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var path = Path.Combine(_imagePath, fileName);

        using var stream = File.Create(path);
        await image.CopyToAsync(stream, cancellationToken);

        return $"/Images/{fileName}";
    }

    public async Task<List<string>> UploadManyImageAsync(List<IFormFile> images, CancellationToken cancellationToken = default)
    {
        List<string> paths = [];

        foreach (var image in images)
        {
            var path = await UploadImageAsync(image, cancellationToken);

            paths.Add(path);
        }

        return paths;
    }

    public Task DeleteImages(List<string> imagePaths)
    {
        foreach (var imagePath in imagePaths)
        {
            // New records contain a public URL. Older records may still contain
            // an absolute Windows path, so support both formats safely.
            var normalizedPath = imagePath.Replace('\\', '/');
            var fileName = normalizedPath[(normalizedPath.LastIndexOf('/') + 1)..];
            var path = Path.Combine(_imagePath, fileName);

            if (File.Exists(path))
                File.Delete(path);
        }

        return Task.CompletedTask;
    }
}
