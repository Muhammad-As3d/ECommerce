using ECommerce.Domain.Common;

namespace ECommerce.Domain.Errors;

public static class CategoryError
{
    public static Error NotFound(int id) =>
        Error.NotFound("Category.NotFound", $"Category with ID '{id}' was not found.");
    public static Error DuplicatedName() =>
        Error.NotFound("Category.DuplicatedName", $"A category with this name already exists.");
}
