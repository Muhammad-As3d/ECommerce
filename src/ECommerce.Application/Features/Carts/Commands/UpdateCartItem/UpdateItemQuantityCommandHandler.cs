namespace ECommerce.Application.Features.Carts.Commands.UpdateCartItem;

internal class UpdateItemQuantityCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateItemQuantityCommand, Result>
{
    public async Task<Result> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var itemExists = await unitOfWork
            .Repository<CartItem>()
            .AnyAsync(x => x.Id == request.CartItemId, cancellationToken);

        if (!itemExists)
            return Result.Failure(CartErrors.CartItemsNotFound);

        var quantityExists = await unitOfWork
            .Repository<CartItem>()
            .AnyAsync(x => x.Id == request.CartItemId && x.Product.Stock >= request.Quantity, cancellationToken);

        if (!quantityExists)
            return Result.Failure(CartErrors.InsufficientStock);

        var item = CartItem.CreateStub(request.CartItemId);
        var changedProperties = item.UpdateQuantity(request.Quantity);

        unitOfWork.Repository<CartItem>().PartialUpdate(item, changedProperties);

        var rowsAffected = await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}