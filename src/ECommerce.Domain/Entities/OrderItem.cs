using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public string ProductNameSnapshot { get; set; } = string.Empty;
    public string? VariantAttributesSnapshot { get; set; }
    public decimal UnitPriceSnapshot { get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; } = null!;

    private OrderItem() { }

    public static OrderItem Create(Guid orderId, string productNameSnapshot, decimal unitPriceSnapshot, int quantity) =>
        new()
        {
            OrderId = orderId,
            UnitPriceSnapshot = unitPriceSnapshot,
            ProductNameSnapshot = productNameSnapshot,
            Quantity = quantity
        };
}
