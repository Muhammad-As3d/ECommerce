using ECommerce.Application.Contracts.Roles;
using ECommerce.Application.Interfaces.Authentication;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
public class RolesController(IRoleService roleService) : ApiBaseController
{
    [HttpGet("")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken)
    {
        var roles = await roleService.GetAllAsync(includeDisabled, cancellationToken);

        return Ok(roles);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await roleService.GetAsync(id, cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Create([FromBody] RoleRequest request, CancellationToken cancellationToken)
    {
        var result = await roleService.AddAsync(request, cancellationToken);

        return HandleResult(result, nameof(Get), r => new { id = r.Id });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RoleRequest request, CancellationToken cancellationToken)
    {
        var result = await roleService.UpdateAsync(id, request, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("{id}/toggle-status")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await roleService.ToggleStatusAsync(id, cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("permissions")]
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken)
    {
        var permissions = await roleService.GetAllPermissionsAsync(cancellationToken);

        return Ok(permissions);
    }
}