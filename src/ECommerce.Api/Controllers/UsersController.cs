using ECommerce.Api.Authorization;
using ECommerce.Application.Contracts.Users;
using ECommerce.Application.Interfaces.Authentication;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
public class UsersController(IUserService userService) : ApiBaseController
{
    [HttpGet("")]
    [HasPermission(Permissions.UsersGetAll)]
    public async Task<IActionResult> GetAll([FromQuery] UserFiltersRequest filters, CancellationToken cancellationToken)
    {
        var users = await userService.GetAllAsync(filters, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.UsersGet)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var result = await userService.GetAsync(id);

        return HandleResult(result);
    }

    [HttpPost("")]
    [HasPermission(Permissions.UsersAdd)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await userService.AddAsync(request, cancellationToken);

        return HandleResult(result, nameof(Get), v => new { id = result.Value.Id });
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UsersUpdate)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await userService.UpdateAsync(id, request, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UsersDisable)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await userService.ToggleStatusAsync(id, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("{id}/unlock")]
    [HasPermission(Permissions.UsersUnlock)]
    public async Task<IActionResult> Unlock([FromRoute] string id)
    {
        var result = await userService.UnlockAsync(id);

        return HandleResult(result);
    }
}