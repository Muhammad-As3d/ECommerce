namespace ECommerce.Application.Contracts.Carts;

public sealed record CartItemCheckoutInfo(
    Guid ProductId,
    int Quantity);
