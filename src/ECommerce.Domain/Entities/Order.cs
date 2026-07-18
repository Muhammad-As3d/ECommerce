using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public Guid ShippingAddressId { get; set; }
    public Guid ShippingMethodId { get; set; }
    public Guid? CouponId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public Address ShippingAddress { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = [];
    public Payment? Payment { get; set; }
}