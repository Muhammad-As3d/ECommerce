using ECommerce.Application.Contracts.Carts;
using ECommerce.Application.Specifications.CartSpecification;

namespace ECommerce.Application.Features.Carts.Queries.GetCart;

public record GetCartQuery() : IRequest<Result<CartUserResponse>>;

public class GetCartQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetCartQuery, Result<CartUserResponse>>
{
    public async Task<Result<CartUserResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cartSpec = new CartSpecification(currentUser.Id);
        var cart = await unitOfWork
           .Repository<Cart>()
           .GetBySpecProjectAsync<CartResponse>(cartSpec, cancellationToken);

        if (cart is null)
            return Result.Success(new CartUserResponse(Guid.Empty, [], 0, 0, 0, 0));

        var itemSpec = new CartItemsSpecification(cart.Id);
        var items = await unitOfWork
            .Repository<CartItem>()
            .GetAllSpecProjectAsync<CartItemResponse>(itemSpec, cancellationToken);

        var subtotal = items!.Sum(x => x.Subtotal);
        const decimal discount = 0; // Discount calc from Coupon db  

        var response = new CartUserResponse(cart.Id, items!, subtotal, discount, subtotal - discount, cart.ItemCount);

        return Result.Success(response);
    }
}