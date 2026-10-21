namespace ECommerce.Infrastructure.Implementations.Authentication;

public interface IJwtProvider
{
    (string token, int ExpiresIn) GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? ValidateToken(string token);
}
