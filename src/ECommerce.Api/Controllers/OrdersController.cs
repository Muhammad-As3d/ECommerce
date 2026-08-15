using ECommerce.Api.ViewModels.Orders;
using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Orders.Commands.CancelOrder;
using ECommerce.Application.Features.Orders.Commands.OrderCheckout;
using ECommerce.Application.Features.Orders.Commands.StartProcessing;
using ECommerce.Application.Features.Orders.Queries.GetAllOrders;
using ECommerce.Application.Features.Orders.Queries.GetOrder;
using ECommerce.Application.Features.Orders.Queries.GetOrders;
using ECommerce.Infrastructure.Identity.Seeding;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/v1/")]
public class OrdersController(ISender sender) : ApiBaseController
{
    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpGet("admin/orders")]
    public async Task<IActionResult> GetAll([FromQuery] FiltersRequest filters, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllOrdersQuery(filters), cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = DefaultRoles.Customer.Name)]
    [HttpGet("orders")]
    public async Task<IActionResult> GetAllByUser([FromQuery] FiltersRequest filters, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrdersQuery(filters), cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = DefaultRoles.Customer.Name)]
    [HttpGet("orders/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrderByIdQuery(id), cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Customer.Name)]
    [HttpPost("orders/checkout")]
    public async Task<IActionResult> Checkout([FromBody] OrderRequest request, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(request.Adapt<OrderCheckoutCommand>(), cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Customer.Name)]
    [HttpPost("orders/{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelOrderRequest request, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CancelOrderCommand(id, request.Reason), cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPost("admin/orders/{id:guid}/process")]
    public async Task<IActionResult> StartProcessing(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new StartProcessingCommand(id), cancellationToken);

        return HandleResult(result);
    }
}
