using ECommerce.Api.ViewModels.Cart;
using ECommerce.Application.Features.Carts.Commands.AddCartItem;
using ECommerce.Application.Features.Carts.Commands.ClearCart;
using ECommerce.Application.Features.Carts.Commands.DeleteCartItem;
using ECommerce.Application.Features.Carts.Commands.UpdateCartItem;
using ECommerce.Application.Features.Carts.Queries.GetCart;
using ECommerce.Infrastructure.Identity.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Authorize(Roles = DefaultRoles.Customer.Name)]
[Route("api/v1/[controller]")]
[ApiController]
public class CartsController(ISender sender) : ApiBaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetCartQuery(), cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("item")]
    public async Task<IActionResult> AddItems(CartItemRequest request, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new AddCartItemCommand(request.ProductId, request.Quantity), cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("item/{id:Guid}")]
    public async Task<IActionResult> UpdateItem(Guid id, UpdateItemRequest request, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new UpdateItemQuantityCommand(id, request.Quantity), cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("item/{id:Guid}")]
    public async Task<IActionResult> DeleteItem(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteCartItemCommand(id), cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new ClearCartCommand(), cancellationToken);

        return HandleResult(result);
    }
}
