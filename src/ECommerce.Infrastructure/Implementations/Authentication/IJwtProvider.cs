namespace ECommerce.Infrastructure.Implementations.Authentication;

public interface IJwtProvider
{
    Task<(string token, int ExpiresIn)> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles);
    string? ValidateToken(string token);
}
