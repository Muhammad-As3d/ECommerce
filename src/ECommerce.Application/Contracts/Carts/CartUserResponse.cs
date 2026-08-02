namespace ECommerce.Application.Contracts.Carts;

public record CartUserResponse(
    Guid Id,
    IEnumerable<CartItemResponse> CartItems,
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    int ItemCount
);

public record CartResponse(
    Guid Id,
    int ItemCount
);

public record CartItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ImageUrl,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal,
    bool IsInStock
);