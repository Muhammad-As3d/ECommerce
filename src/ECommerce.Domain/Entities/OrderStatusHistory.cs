using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class OrderStatusHistory : BaseEntity
{
    public Guid OrderId { get; private set; }
    public OrderStatus? FromStatus { get; private set; }
    public OrderStatus ToStatus { get; private set; }

    public string? ChangedById { get; private set; }
    public string? Reason { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }

    public Order Order { get; private set; } = null!;

    private OrderStatusHistory() { }

    public static OrderStatusHistory Create(Guid orderId, OrderStatus? from, OrderStatus to, string? changedById,
        string? reason) =>
         new()
         {
             OrderId = orderId,
             FromStatus = from,
             ToStatus = to,
             ChangedById = changedById,
             Reason = reason,
             ChangedAt = DateTimeOffset.UtcNow
         };

}