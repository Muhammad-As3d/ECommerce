namespace ECommerce.Domain.Enums;

public enum PaymentStatus : byte
{
    Pending = 1,
    RequiresAction = 2,
    Processing = 3,
    Succeeded = 4,
    Failed = 5,
    Cancelled = 6,
    PartiallyRefunded = 7,
    Refunded = 8
}

public enum PaymentMethod : byte
{
    CashOnDelivery = 1,
    Card = 2
}

public enum PaymentProvider : byte
{
    None = 0,
    Stripe = 1
}

public enum RefundStatus : byte
{
    Pending = 1,
    Succeeded = 2,
    Failed = 3,
    Cancelled = 4
}