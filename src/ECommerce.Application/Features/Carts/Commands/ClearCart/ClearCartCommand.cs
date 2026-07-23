namespace ECommerce.Application.Features.Carts.Commands.ClearCart;

public record ClearCartCommand() : IRequest<Result>;

internal class ClearCartCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<ClearCartCommand, Result>
{
    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var rowsAffected = await unitOfWork
            .Repository<CartItem>()
            .DeleteAsync(x => x.Cart.UserId == currentUser.Id, cancellationToken);

        return Result.Success();
    }
}