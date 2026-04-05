using ECommerce.Domain.Common;

namespace ECommerce.Domain.Errors;

public static class ProductErrors
{
    public static Error NotFound(int id) =>
        Error.NotFound("Product.NotFound", $"Product with ID '{id}' was not found");
}
