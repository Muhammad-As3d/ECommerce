namespace ECommerce.Application.Features.Wishlists.Commands.AddWishlistItem;

public record AddWishlistItemCommand(
    Guid ProductId
) : IRequest<Result<Guid>>;
