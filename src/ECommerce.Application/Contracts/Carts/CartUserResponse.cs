namespace ECommerce.Application.Contracts.Carts;

public record CartUserResponse(
    Guid Id,
    List<CartItemResponse> CartItems,
    double Subtotal,
    double Discount,
    double Total,
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
    double UnitPrice,
    int Quantity,
    double Subtotal,
    bool IsInStock
);