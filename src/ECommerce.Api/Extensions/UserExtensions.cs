using System.Security.Claims;

namespace ECommerce.Api.Extensions;

public static class UserExtensions
{
    public static string? GetUser(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}
