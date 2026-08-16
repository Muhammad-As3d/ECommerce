namespace ECommerce.Application.Contracts.Orders;

public sealed record MarkOrderAsDeliveredResponse(
    Guid Id,
    OrderStatus Status,
    DateTimeOffset DeliveredAt);

public sealed record MarkOrderAsDeliveredInfo(
    Guid Id,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    byte[] RowVersion,
    CashPaymentInfo? CashPayment);

public sealed record CashPaymentInfo(
    Guid Id,
    PaymentMethod Method,
    PaymentStatus Status,
    byte[] RowVersion);
