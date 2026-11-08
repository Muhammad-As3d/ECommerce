using ECommerce.Api.Extensions;
using ECommerce.Application.Contracts.Users;
using ECommerce.Application.Interfaces.Authentication;

namespace ECommerce.Api.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IAccountService accountService) : ApiBaseController
{
    [HttpGet("")]
    public async Task<IActionResult> Info(CancellationToken cancellationToken = default)
    {
        var result = await accountService.GetProfileAsync(User.GetUser()!, cancellationToken);

        return Ok(result);
    }

    [HttpPut("info")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var result = await accountService.UpdateProfileAsync(User.GetUser()!, request, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await accountService.ChangePasswordAsync(User.GetUser()!, request, cancellationToken);

        return HandleResult(result);
    }
}