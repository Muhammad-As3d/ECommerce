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

    public async Task<List<string>> UploadManyImageAsync(List<IFormFile> images, CancellationToken cancellationToken = default)
    {
        List<string> paths = [];

        foreach (var image in images)
        {
            var path = Path.Combine(_imagePath, image.FileName);

            using var stream = File.Create(path);
            await image.CopyToAsync(stream, cancellationToken);

            paths.Add(path);
        }

        return paths;
    }


    public Task DeleteImages(List<string> imagePaths)
    {
        foreach (var path in imagePaths)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        return Task.CompletedTask;
    }
}
