namespace ECommerce.Application.Interfaces.Services;

public interface IJwtProvider
{
    Task<(string token, int ExpiresIn)> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles);
}
