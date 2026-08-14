namespace ECommerce.Domain.Enums;

public enum OrderStatus : byte
{
    PendingPayment = 1,
    Confirmed = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6,
    PaymentFailed = 7,
    ReturnRequested = 8,
    Returned = 9,
    Refunded = 10
}