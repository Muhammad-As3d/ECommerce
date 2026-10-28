using ECommerce.Application.Contracts.Users;
using ECommerce.Application.Features.Users.Commands.ChangePassword;
using ECommerce.Application.Features.Users.Commands.UpdateProfile;
using ECommerce.Application.Features.Users.Queries.GetProfile;

namespace ECommerce.Api.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(ISender sender) : ApiBaseController
{
    [HttpGet("")]
    public async Task<IActionResult> Info(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetProfileQuery(), cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("info")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateProfileCommand(request), cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ChangePasswordCommand(request), cancellationToken);

        return HandleResult(result);
    }
}