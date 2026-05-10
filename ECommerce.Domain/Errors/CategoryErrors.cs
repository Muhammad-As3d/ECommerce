using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class CategoryErrors
{
    public static Error NotFound(int id) =>
        Error.NotFound("Category.NotFound", $"Category with ID '{id}' was not found.");
    public static Error DuplicatedName =>
        Error.Conflict("Category.DuplicatedName", "A category with this name already exists.");
}
