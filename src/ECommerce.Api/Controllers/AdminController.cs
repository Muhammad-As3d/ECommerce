using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Orders.Queries.GetAllOrders;
using ECommerce.Infrastructure.Identity.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = DefaultRoles.Admin.Name)]
public class AdminController(ISender sender) : ApiBaseController
{
    [HttpGet("orders")]
    public async Task<IActionResult> GetAllOrders([FromQuery] FiltersRequest filters, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllOrdersQuery(filters), cancellationToken);

        return Ok(result);
    }
}
