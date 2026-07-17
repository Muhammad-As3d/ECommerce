using ECommerce.Application.Contracts.Authentication;
using ECommerce.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ApiBaseController
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("confirmation-email")]
    public async Task<IActionResult> ConfirmationEmail([FromQuery] ConfirmationEmailRequest request)
    {
        var result = await _authService.ConfirmationEmailAsync(request);

        return HandleResult(result);
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);

        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return HandleResult(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken);

        return HandleResult(result);
    }
    [HttpPost("revoke-refresh")]
    public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken);

        return HandleResult(result);
    }
}
