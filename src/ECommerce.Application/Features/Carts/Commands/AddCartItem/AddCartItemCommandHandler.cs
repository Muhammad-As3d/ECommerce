namespace ECommerce.Application.Features.Carts.Commands.AddCartItem;

internal class AddCartItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<AddCartItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var productRepository = unitOfWork.Repository<Product>();
        var cartRepository = unitOfWork.Repository<Cart>();
        var cartItemRepository = unitOfWork.Repository<CartItem>();

        var product = await productRepository
            .GetByPredicateProjectAsync(
            x => x.Id == request.ProductId && !x.IsDeleted && x.IsActive,
            x => new ProductCartInfo(
                x.Stock,
                x.Price), cancellationToken);

        if (product is null)
            return Result.Failure<Guid>(ProductErrors.NotFound(request.ProductId));

        if (product.Stock < request.Quantity)
            return Result.Failure<Guid>(CartErrors.QuantityNotAvailable);

        var cartId = await cartRepository
            .GetByPredicateProjectAsync(x => x.UserId == currentUser.Id, x => (Guid?)x.Id,
            cancellationToken);

        if (cartId is null)
        {
            var cart = Cart.Create(currentUser.Id);
            await cartRepository.AddAsync(cart, cancellationToken);

            cartId = cart.Id;
        }

        var productAlreadyExists = await cartItemRepository
            .AnyAsync(x => x.ProductId == request.ProductId && x.CartId == cartId.Value, cancellationToken);

        if (productAlreadyExists)
            return Result.Failure<Guid>(CartErrors.ProductIsOnCart);

        var cartItem = CartItem.Create(cartId.Value, request.ProductId, request.Quantity, product.Price);
        await cartItemRepository.AddAsync(cartItem, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(cartId.Value);
    }
}