using ECommerce.Application.Specifications.WishlistSpecification;

namespace ECommerce.Application.Features.Wishlists.Queries.GetWishlist;

public record GetWishlistQuery() : IRequest<Result<WishlistUserResponse>>;

public class GetWishlistQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetWishlistQuery, Result<WishlistUserResponse>>
{
    public async Task<Result<WishlistUserResponse>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
    {
        var wishlistSpec = new WishlistSpecification(currentUser.Id);

        var wishlist = await unitOfWork
           .Repository<Wishlist>()
           .GetBySpecProjectAsync<WishlistResponse>(wishlistSpec, cancellationToken);

        if (wishlist is null)
            return Result.Success(new WishlistUserResponse(Guid.Empty, [], 0));

        var itemSpec = new WishlistItemsSpecification(wishlist.Id);
        var items = await unitOfWork
            .Repository<WishlistItem>()
            .GetAllSpecProjectAsync<WishlistItemResponse>(itemSpec, cancellationToken);

        var response = new WishlistUserResponse(wishlist.Id, items!, wishlist.ItemCount);

        return Result.Success(response);
    }
}
