namespace ECommerce.Application.Features.Wishlists.Commands.ClearWishlist;

public record ClearWishlistCommand() : IRequest<Result>;

internal class ClearWishlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<ClearWishlistCommand, Result>
{
    public async Task<Result> Handle(ClearWishlistCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork
            .Repository<WishlistItem>()
            .DeleteAsync(x => x.Wishlist.UserId == currentUser.Id, cancellationToken);

        return Result.Success();
    }
}
