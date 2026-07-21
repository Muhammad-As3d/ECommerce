using ECommerce.Application.Contracts.Carts;

namespace ECommerce.Application.Features.Carts.Commands.AddCartItem;

internal class AddCartItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<AddCartItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var itemRepo = unitOfWork.Repository<CartItem>();
        var cartRepo = unitOfWork.Repository<Cart>();

        var product = await unitOfWork
            .Repository<Product>()
            .GetByIdProjectAsync<CartProductResponse>(request.ProductId, cancellationToken);

        if (product is null)
            return Result.Failure<Guid>(ProductErrors.NotFound(request.ProductId));

        if (request.Quantity > product.Stock)
            return Result.Failure<Guid>(CartErrors.QuantityNotAvailable);

        var cart = await cartRepo.GetByPredicateAsync(x => x.UserId == currentUser.Id, cancellationToken);

        if (cart is null)
        {
            cart = Cart.Create(currentUser.Id);
            await cartRepo.AddAsync(cart, cancellationToken);
        }

        var productIsExistsInItem = await itemRepo
            .AnyAsync(x => x.ProductId == request.ProductId && x.CartId == cart.Id, cancellationToken);

        if (productIsExistsInItem)
            return Result.Failure<Guid>(CartErrors.ProductIsOnCart);

        var cartItem = CartItem.Create(cart.Id, request.ProductId, request.Quantity, product.Price);
        await itemRepo.AddAsync(cartItem, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(cart.Id);
    }
}