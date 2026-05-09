using ECommerce.Application.Contracts.Authentication;
using ECommerce.Domain.Abstractions;

namespace ECommerce.Application.Interfaces.Authentication;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result> ConfirmationEmailAsync(ConfirmationEmailRequest request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
