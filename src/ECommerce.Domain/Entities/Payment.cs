using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Payment : AuditableEntity
{
    public Guid OrderId { get; set; }
    public string Provider { get; set; } = "Stripe";
    public string? ProviderTransactionId { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public decimal Amount { get; set; }
    public DateTime? PaidAt { get; set; }

    public Order Order { get; set; } = null!;
}
