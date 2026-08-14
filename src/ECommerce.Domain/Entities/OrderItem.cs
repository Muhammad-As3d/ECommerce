using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid? ProductId { get; private set; }

<<<<<<< Updated upstream
    public Order Order { get; set; } = null!;
=======
    public string ProductNameSnapshot { get; private set; } = string.Empty;
    public string? SkuSnapshot { get; private set; }
    public string? VariantAttributesSnapshot { get; private set; }

    public decimal UnitPriceSnapshot { get; private set; }
    public decimal DiscountSnapshot { get; private set; }
    public decimal TaxSnapshot { get; private set; }

    public int Quantity { get; private set; }

    public decimal LineTotal => (UnitPriceSnapshot * Quantity) - DiscountSnapshot + TaxSnapshot;

    public Order Order { get; private set; } = null!;
    public Product? Product { get; private set; }

    private OrderItem() { }

    internal static OrderItem Create(Guid orderId, Guid productId, string productName, string? sku, decimal unitPrice, int quantity)
        =>
         new()
         {
             OrderId = orderId,
             ProductId = productId,
             ProductNameSnapshot = productName,
             SkuSnapshot = sku,
             UnitPriceSnapshot = unitPrice,
             Quantity = quantity
         };
>>>>>>> Stashed changes
}
