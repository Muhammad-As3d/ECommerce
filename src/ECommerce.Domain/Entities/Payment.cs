using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Payment : AuditableEntity
{
    private readonly List<Refund> _refunds = [];

    public Guid OrderId { get; private set; }

    public PaymentMethod Method { get; private set; }
    public PaymentProvider Provider { get; private set; }
    public PaymentStatus Status { get; private set; }

    public decimal Amount { get; private set; }
    public decimal RefundedAmount { get; private set; }
    public string Currency { get; private set; } = "EGP";

    public string? ProviderPaymentIntentId { get; private set; }
    public string? ProviderChargeId { get; private set; }

    public string? FailureCode { get; private set; }
    public string? FailureMessage { get; private set; }

    public DateTimeOffset? PaidAt { get; private set; }
    public DateTimeOffset? FailedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }

    public byte[] RowVersion { get; private set; } = [];

    public Order Order { get; private set; } = null!;
    public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();

    private Payment() { }

    public static Payment CreateStub(Guid id, byte[] rowVersion, PaymentMethod method, PaymentStatus status)
    {
        if (rowVersion is null || rowVersion.Length == 0)
            throw new ArgumentException("RowVersion is required.", nameof(rowVersion));

        return new Payment
        {
            Id = id,
            RowVersion = rowVersion,
            Method = method,
            Status = status
        };
    }

    public static Payment CreateCash(Guid orderId, decimal amount, string currency)
    {
        return new Payment
        {
            OrderId = orderId,
            Method = PaymentMethod.CashOnDelivery,
            Provider = PaymentProvider.None,
            Status = PaymentStatus.Pending,
            Amount = amount,
            Currency = currency.ToUpperInvariant()
        };
    }

    public static Payment CreateStripe(
        Guid orderId,
        decimal amount,
        string currency,
        string paymentIntentId)
    {
        return new Payment
        {
            OrderId = orderId,
            Method = PaymentMethod.Card,
            Provider = PaymentProvider.Stripe,
            Status = PaymentStatus.Pending,
            Amount = amount,
            Currency = currency.ToUpperInvariant(),
            ProviderPaymentIntentId = paymentIntentId
        };
    }

    public void RequiresCustomerAction()
    {
        Status = PaymentStatus.RequiresAction;
    }

    public void MarkProcessing()
    {
        Status = PaymentStatus.Processing;
    }

    public void MarkSucceeded(
        string? providerChargeId,
        DateTimeOffset paidAt)
    {
        if (Status == PaymentStatus.Succeeded)
            return; // idempotent webhook handling

        if (Status is PaymentStatus.Cancelled or PaymentStatus.Refunded)
            throw new InvalidOperationException("Invalid payment transition.");

        Status = PaymentStatus.Succeeded;
        ProviderChargeId = providerChargeId;
        PaidAt = paidAt;
        FailureCode = null;
        FailureMessage = null;
    }

    public void MarkFailed(string? code, string? message)
    {
        if (Status == PaymentStatus.Succeeded)
            throw new InvalidOperationException(
                "Successful payment cannot be marked as failed.");

        Status = PaymentStatus.Failed;
        FailureCode = code;
        FailureMessage = message;
        FailedAt = DateTimeOffset.UtcNow;
    }

    public IReadOnlyCollection<string> MarkCashCollected()
    {
        if (Method != PaymentMethod.CashOnDelivery)
            throw new InvalidOperationException("Payment is not cash.");

        MarkSucceeded(null, DateTimeOffset.UtcNow);

        return
        [
            nameof(Status),
            nameof(ProviderChargeId),
            nameof(PaidAt),
            nameof(FailureCode),
            nameof(FailureMessage)
        ];
    }

    public void ApplyRefund(decimal amount)
    {
        if (Status is not (
            PaymentStatus.Succeeded or
            PaymentStatus.PartiallyRefunded))
        {
            throw new InvalidOperationException(
                "Only successful payments can be refunded.");
        }

        if (amount <= 0 || RefundedAmount + amount > Amount)
            throw new InvalidOperationException("Invalid refund amount.");

        RefundedAmount += amount;

        Status = RefundedAmount == Amount
            ? PaymentStatus.Refunded
            : PaymentStatus.PartiallyRefunded;
    }
}
