namespace ECommerce.Api.ViewModels.Cart;

public record CartItemRequest(
    Guid ProductId,
    int Quantity
);

public record UpdateItemRequest(
    int Quantity
);