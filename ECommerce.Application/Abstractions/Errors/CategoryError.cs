using ECommerce.Application.Abstractions.Results;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Abstractions.Errors;

public static class CategoryError
{
    public static readonly Error CategoryNotFound =
        new("Category.NotFound", "Not Found Category with the given ID", StatusCodes.Status404NotFound);
}
