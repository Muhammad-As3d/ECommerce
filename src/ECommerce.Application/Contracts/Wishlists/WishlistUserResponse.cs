namespace ECommerce.Application.Contracts.Wishlists;

public record WishlistUserResponse(
    Guid Id,
    IEnumerable<WishlistItemResponse> Items,
    int ItemCount
);

public record WishlistResponse(
    Guid Id,
    int ItemCount
);

public record WishlistItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ImageUrl,
    decimal Price,
    bool IsInStock
);
