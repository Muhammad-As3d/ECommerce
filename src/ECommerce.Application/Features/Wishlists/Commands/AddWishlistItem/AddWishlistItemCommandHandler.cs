namespace ECommerce.Application.Features.Wishlists.Commands.AddWishlistItem;

internal class AddWishlistItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<AddWishlistItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddWishlistItemCommand request, CancellationToken cancellationToken)
    {
        var productRepository = unitOfWork.Repository<Product>();
        var wishlistRepository = unitOfWork.Repository<Wishlist>();
        var wishlistItemRepository = unitOfWork.Repository<WishlistItem>();

        var productExists = await productRepository
            .AnyAsync(x => x.Id == request.ProductId && !x.IsDeleted && x.IsActive, cancellationToken);

        if (!productExists)
            return ProductErrors.NotFound(request.ProductId);

        var wishlistId = await wishlistRepository
            .GetByPredicateProjectAsync(x => x.UserId == currentUser.Id, x => (Guid?)x.Id,
            cancellationToken);

        if (wishlistId is null)
        {
            var wishlist = Wishlist.Create(currentUser.Id);
            await wishlistRepository.AddAsync(wishlist, cancellationToken);

            wishlistId = wishlist.Id;
        }

        var productAlreadyExists = await wishlistItemRepository
            .AnyAsync(x => x.ProductId == request.ProductId && x.WishlistId == wishlistId.Value, cancellationToken);

        if (productAlreadyExists)
            return WishlistErrors.ProductIsOnWishlist;

        var wishlistItem = WishlistItem.Create(wishlistId.Value, request.ProductId);
        await wishlistItemRepository.AddAsync(wishlistItem, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(wishlistId.Value);
    }
}
