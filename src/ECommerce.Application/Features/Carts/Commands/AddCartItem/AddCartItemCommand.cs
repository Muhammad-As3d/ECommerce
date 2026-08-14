namespace ECommerce.Application.Features.Carts.Commands.AddCartItem;

public record AddCartItemCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<Guid>>;