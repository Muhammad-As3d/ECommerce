using ECommerce.Api.ViewModels.Cart;
using ECommerce.Application.Features.Carts.Commands.AddCartItem;
using ECommerce.Application.Features.Carts.Queries.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(ISender sender) : ApiBaseController
{
    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetCartQuery(), cancellationToken);

        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("item")]
    public async Task<IActionResult> AddItems(CartItemRequest request, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new AddCartItemCommand(request.ProductId, request.Quantity), cancellationToken);

        return HandleResult(result);
    }
}