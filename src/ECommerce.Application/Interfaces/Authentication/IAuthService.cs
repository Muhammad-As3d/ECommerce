using ECommerce.Application.Contracts.Authentication;

namespace ECommerce.Application.Interfaces.Authentication;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result> ConfirmationEmailAsync(ConfirmationEmailRequest request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken);
}
