namespace ECommerce.Application.Contracts.Carts;

public record CartProductResponse(int Stock, decimal Price);
public record ProductStockResponse(int Stock);