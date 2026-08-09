namespace ECommerce.Application.Features.Carts.Commands.DeleteCartItem;

public record DeleteCartItemCommand(Guid CartItemId) : IRequest<Result>;

internal class DeleteCartItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteCartItemCommand, Result>
{
    public async Task<Result> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
    {
        var rowsAffected = await unitOfWork
            .Repository<CartItem>()
            .DeleteAsync(x => x.Id == request.CartItemId && x.Cart.UserId == currentUser.Id, cancellationToken);

        return rowsAffected > 0
            ? Result.Success()
            : Result.Failure(CartErrors.ItemsNotFound);
    }
}