namespace ECommerce.Api.ViewModels.Cart;

public record CartItemRequest(
    Guid ProductId,
    int Quantity
);