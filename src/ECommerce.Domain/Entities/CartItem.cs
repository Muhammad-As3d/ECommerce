using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class CartItem : BaseEntity
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double UnitPriceSnapshot { get; set; }

    public Cart Cart { get; set; } = default!;
    public Product Product { get; set; } = default!;

    private CartItem() { }

    public static CartItem CreateStub(Guid id) => new() { Id = id };

    public static CartItem Create(Guid cartId, Guid productId, int quantity, double unitPriceSnapshot) =>
        new()
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity,
            UnitPriceSnapshot = unitPriceSnapshot
        };
}
