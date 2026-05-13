using ECommerce.Api.ViewModels;
using ECommerce.Application.Features.Categories.Create;
using ECommerce.Application.Features.Categories.Get;
using ECommerce.Application.Features.Categories.GetAll;
using ECommerce.Application.Features.Categories.ToggleStatus;
using ECommerce.Application.Features.Categories.Update;
using ECommerce.Infrastructure.Identity.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ISender sender) : ApiBaseController
{
    private readonly ISender _sender = sender;

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetAllCategoriesQuery(), cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCategoryByIdQuery(id), cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name, request.Description);

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(id, request.Name, request.Description);

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ToggleStatusCategoryCommand(id), cancellationToken);

        return HandleResult(result);
    }
}
