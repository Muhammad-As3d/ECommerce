using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Refund : AuditableEntity
{
    public Guid PaymentId { get; private set; }

    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "EGP";
    public string Reason { get; private set; } = string.Empty;

    public RefundStatus Status { get; private set; }
    public string? ProviderRefundId { get; private set; }
    public string? FailureMessage { get; private set; }

    public DateTimeOffset? RefundedAt { get; private set; }

    public Payment Payment { get; private set; } = null!;

    private Refund() { }

    public static Refund Create(Guid paymentId, decimal amount, string currency, string reason)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        return new Refund
        {
            PaymentId = paymentId,
            Amount = amount,
            Currency = currency.ToUpperInvariant(),
            Reason = reason,
            Status = RefundStatus.Pending
        };
    }

    public void MarkSucceeded(string providerRefundId, DateTimeOffset refundedAt)
    {
        Status = RefundStatus.Succeeded;
        ProviderRefundId = providerRefundId;
        RefundedAt = refundedAt;
    }

    public void MarkFailed(string message)
    {
        Status = RefundStatus.Failed;
        FailureMessage = message;
    }
}