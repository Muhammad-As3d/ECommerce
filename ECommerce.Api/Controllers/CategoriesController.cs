using ECommerce.Application.Features.Categories.GetAll;
using ECommerce.Infrastructure.Identity.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetAllCategoriesQuery(), cancellationToken);

        return Ok(response);
    }
}
