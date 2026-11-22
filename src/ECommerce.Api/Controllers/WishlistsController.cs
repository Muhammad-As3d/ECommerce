using ECommerce.Api.ViewModels.Wishlist;
using ECommerce.Application.Features.Wishlists.Commands.AddWishlistItem;
using ECommerce.Application.Features.Wishlists.Commands.ClearWishlist;
using ECommerce.Application.Features.Wishlists.Commands.DeleteWishlistItem;
using ECommerce.Application.Features.Wishlists.Queries.GetWishlist;

namespace ECommerce.Api.Controllers;

[Authorize(Roles = DefaultRoles.Customer.Name)]
[Route("api/v1/[controller]")]
[ApiController]
public class WishlistsController(ISender sender) : ApiBaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetWishlistQuery(), cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("item")]
    public async Task<IActionResult> AddItem([FromBody] WishlistItemRequest request, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(request.Adapt<AddWishlistItemCommand>(), cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("item/{id:Guid}")]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteWishlistItemCommand(id), cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearWishlist(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new ClearWishlistCommand(), cancellationToken);

        return HandleResult(result);
    }
}
