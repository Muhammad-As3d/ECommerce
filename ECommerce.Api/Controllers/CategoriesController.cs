using ECommerce.Api.ViewModels;
using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Categories.Commands.Create;
using ECommerce.Application.Features.Categories.Commands.ToggleStatus;
using ECommerce.Application.Features.Categories.Commands.Update;
using ECommerce.Application.Features.Categories.Queries.Get;
using ECommerce.Application.Features.Categories.Queries.GetAll;
using ECommerce.Application.Features.Categories.Queries.GetCategoryProducts;
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
    public async Task<IActionResult> GetAll([FromQuery] SpecFilters request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetAllCategoriesQuery(request), cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCategoryByIdQuery(id), cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetCategoryProducts([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCategoryProductsQuery(id), cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name, request.Description);

        var result = await _sender.Send(command, cancellationToken);

        return HandleCreatedResult(result, nameof(Get), new { id = result.Value });
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
