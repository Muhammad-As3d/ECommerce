using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class ImageErrors
{
    public static Error NotFound =>
       Error.NotFound("ImagePath.NotFound", $"Image path was not found.");
}
