using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order : AuditableEntity
{
    private readonly List<OrderItem> _items = [];
    private readonly List<Payment> _payments = [];
    private readonly List<OrderStatusHistory> _statusHistory = [];

    public string UserId { get; private set; } = string.Empty;
    public string OrderNumber { get; private set; } = string.Empty;

    public OrderStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    public decimal SubTotal { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount { get; private set; }

    public string Currency { get; private set; } = "EGP";

    public ShippingAddressSnapshot ShippingAddress { get; private set; } = null!;

    public DateTimeOffset? EstimatedDeliveryFrom { get; private set; }
    public DateTimeOffset? EstimatedDeliveryTo { get; private set; }
    public DateTimeOffset? ConfirmedAt { get; private set; }
    public DateTimeOffset? ShippedAt { get; private set; }
    public DateTimeOffset? DeliveredAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }
    public string? TrackingNumber { get; private set; }
    public string? ShippingProvider { get; private set; }

    // Optimistic concurrency
    public byte[] RowVersion { get; private set; } = [];

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();
    public IReadOnlyCollection<OrderStatusHistory> StatusHistory => _statusHistory.AsReadOnly();

    private Order() { }

    public static Order CreateStub(Guid id, byte[] rowVersion, OrderStatus status = default)
    {
        if (rowVersion is null || rowVersion.Length == 0)
            throw new ArgumentException("RowVersion is required.", nameof(rowVersion));

        return new Order
        {
            Id = id,
            RowVersion = rowVersion,
            Status = status
        };
    }

    public static Order Create(string userId, string orderNumber, PaymentMethod paymentMethod, ShippingAddressSnapshot shippingAddress,
        string currency = "EGP", decimal discountAmount = 0, decimal shippingFee = 0, decimal taxAmount = 0)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User is required.");

        if (discountAmount < 0 || shippingFee < 0 || taxAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(discountAmount));

        return new Order
        {
            UserId = userId,
            OrderNumber = orderNumber,
            PaymentMethod = paymentMethod,
            ShippingAddress = shippingAddress,
            Currency = currency.ToUpperInvariant(),
            DiscountAmount = discountAmount,
            ShippingFee = shippingFee,
            TaxAmount = taxAmount,
            Status = paymentMethod == PaymentMethod.Card ? OrderStatus.PendingPayment : OrderStatus.Confirmed,
            ConfirmedAt = paymentMethod == PaymentMethod.CashOnDelivery ? DateTimeOffset.UtcNow : null
        };
    }

    public void AddItem(Guid productId, string productName, string? sku, decimal unitPrice, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);

        _items.Add(OrderItem.Create(Id, productId, productName, sku, unitPrice, quantity));

        RecalculateTotals();
    }

    public void ConfirmPayment()
    {
        if (PaymentMethod != PaymentMethod.Card)
            throw new InvalidOperationException("Order is not a card order.");

        if (Status != OrderStatus.PendingPayment &&
            Status != OrderStatus.PaymentFailed)
            throw new InvalidOperationException("Invalid order transition.");

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTimeOffset.UtcNow;
    }

    public void AddPayment(Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        if (payment.OrderId != Id)
            throw new InvalidOperationException("Payment belongs to another order.");

        if (_payments.Any(x => x.Status == PaymentStatus.Succeeded))
            throw new InvalidOperationException("Order already has a successful payment.");

        _payments.Add(payment);
    }

    public IReadOnlyCollection<string> StartProcessing()
    {
        EnsureStatus(OrderStatus.Confirmed);
        Status = OrderStatus.Processing;

        return [nameof(Status)];
    }

    public IReadOnlyCollection<string> MarkShipped(DateTimeOffset estimatedFrom, DateTimeOffset estimatedTo, string trackingNumber,
        string shippingProvider)
    {
        EnsureStatus(OrderStatus.Processing);

        if (estimatedTo < estimatedFrom)
            throw new ArgumentException("Invalid delivery range.");

        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new ArgumentException("Tracking number is required.", nameof(trackingNumber));

        if (string.IsNullOrWhiteSpace(shippingProvider))
            throw new ArgumentException("Shipping provider is required.", nameof(shippingProvider));

        Status = OrderStatus.Shipped;
        ShippedAt = DateTimeOffset.UtcNow;
        EstimatedDeliveryFrom = estimatedFrom;
        EstimatedDeliveryTo = estimatedTo;
        TrackingNumber = trackingNumber.Trim();
        ShippingProvider = shippingProvider.Trim();

        return
        [
            nameof(Status),
            nameof(ShippedAt),
            nameof(EstimatedDeliveryFrom),
            nameof(EstimatedDeliveryTo),
            nameof(TrackingNumber),
            nameof(ShippingProvider)
        ];
    }

    public IReadOnlyCollection<string> MarkDelivered()
    {
        EnsureStatus(OrderStatus.Shipped);
        Status = OrderStatus.Delivered;
        DeliveredAt = DateTimeOffset.UtcNow;

        return [nameof(Status), nameof(DeliveredAt)];
    }

    public void MarkPaymentFailed()
    {
        if (Status != OrderStatus.PendingPayment)
            throw new InvalidOperationException("Invalid order transition.");

        Status = OrderStatus.PaymentFailed;
    }

    public IReadOnlyCollection<string> Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required.", nameof(reason));

        if (Status is OrderStatus.Shipped or OrderStatus.Delivered or OrderStatus.Cancelled or OrderStatus.Refunded)
            throw new InvalidOperationException("Order cannot be cancelled in its current status.");

        Status = OrderStatus.Cancelled;
        CancellationReason = reason.Trim();
        CancelledAt = DateTimeOffset.UtcNow;

        return [nameof(Status), nameof(CancellationReason), nameof(CancelledAt)];
    }

    private void RecalculateTotals()
    {
        SubTotal = _items.Sum(x => x.LineTotal);

        TotalAmount = Math.Max(0, SubTotal - DiscountAmount + ShippingFee + TaxAmount);
    }

    private void EnsureStatus(OrderStatus expected)
    {
        if (Status != expected)
            throw new InvalidOperationException(
                $"Expected status {expected}, current status is {Status}.");
    }
}
