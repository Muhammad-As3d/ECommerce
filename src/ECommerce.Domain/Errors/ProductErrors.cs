using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class ProductErrors
{
    public static Error NotFound(int id) =>
        Error.NotFound("Product.NotFound", $"Product with ID '{id}' was not found");

    public static readonly Error NotfoundProductImages =
        Error.NotFound("Product.NotfoundProductImages", $"Product has not any images or productId is false.");
}
