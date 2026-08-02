using ECommerce.Application.Features.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class OrdersController(ISender sender) : ApiBaseController
{
    [HttpGet("checkout")]
    public async Task<IActionResult> Checkout([FromQuery] Guid shippingAddressId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new OrderCheckoutCommand(shippingAddressId), cancellationToken);

        return HandleResult(result);
    }
}
