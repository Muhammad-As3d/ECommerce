namespace ECommerce.Application.Contracts.Payments;

public record PaymentWebhookInfo(
    Guid Id,
    byte[] RowVersion,
    PaymentStatus Status,
    PaymentMethod Method,
    decimal Amount,
    Guid OrderId,
    byte[] OrderRowVersion,
    OrderStatus OrderStatus,
    PaymentMethod OrderPaymentMethod
);
