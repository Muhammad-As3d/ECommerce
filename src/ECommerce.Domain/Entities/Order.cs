using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order : AuditableEntity
{
    public string UserId { get; private set; } = string.Empty;
    public Guid ShippingAddressId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal SubTotal { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public int WithinDays { get; private set; } = 3;

    public Address ShippingAddress { get; private set; } = null!;
    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Payment? Payment { get; private set; }

    private Order() { }

    public static Order CreateStub(Guid id) => new() { Id = id };

    public static Order Create(string userId, Guid shippingAddressId, decimal subTotal, string orderNumber,
        decimal discountAmount = 0, decimal shippingFee = 50.00m, decimal taxAmount = 0)
    {
        var order = new Order
        {
            UserId = userId,
            ShippingAddressId = shippingAddressId,
            SubTotal = subTotal,
            DiscountAmount = discountAmount,
            ShippingFee = shippingFee,
            TaxAmount = taxAmount,
            OrderNumber = orderNumber
        };

        order.TotalAmount = order.CalculateTotal();
        return order;
    }
    public void AddItem(OrderItem item) => _items.Add(item);
    private decimal CalculateTotal() => SubTotal - DiscountAmount + ShippingFee + TaxAmount;
}