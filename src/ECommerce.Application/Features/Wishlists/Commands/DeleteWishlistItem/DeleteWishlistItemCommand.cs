namespace ECommerce.Application.Features.Wishlists.Commands.DeleteWishlistItem;

public record DeleteWishlistItemCommand(Guid WishlistItemId) : IRequest<Result>;

internal class DeleteWishlistItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteWishlistItemCommand, Result>
{
    public async Task<Result> Handle(DeleteWishlistItemCommand request, CancellationToken cancellationToken)
    {
        var rowsAffected = await unitOfWork
            .Repository<WishlistItem>()
            .DeleteAsync(x => x.Id == request.WishlistItemId && x.Wishlist.UserId == currentUser.Id, cancellationToken);

        return rowsAffected > 0
            ? Result.Success()
            : WishlistErrors.ItemNotFound;
    }
}
