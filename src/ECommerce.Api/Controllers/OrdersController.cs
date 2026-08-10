using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Orders.Commands;
using ECommerce.Application.Features.Orders.Queries.GetOrder;
using ECommerce.Application.Features.Orders.Queries.GetOrders;
using ECommerce.Infrastructure.Identity.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = DefaultRoles.Customer.Name)]
public class OrdersController(ISender sender) : ApiBaseController
{
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromQuery] Guid shippingAddressId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new OrderCheckoutCommand(shippingAddressId), cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] FiltersRequest filters, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrdersQuery(filters), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrderByIdQuery(id), cancellationToken);

        return HandleResult(result);
    }
}
