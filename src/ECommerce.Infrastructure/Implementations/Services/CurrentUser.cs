namespace ECommerce.Infrastructure.Implementations.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly ClaimsPrincipal User = httpContextAccessor.HttpContext!.User;
    public string Id => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User is not authenticated.");

    public string FullName => $"{User.FindFirstValue(ClaimTypes.GivenName)} {User.FindFirstValue(ClaimTypes.Surname)}".Trim()
        ?? throw new UnauthorizedAccessException("User is not authenticated.");
}
