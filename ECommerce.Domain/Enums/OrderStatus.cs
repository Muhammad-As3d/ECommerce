namespace ECommerce.Domain.Enums;

public enum OrderStatus : byte
{
    Pending = 1,
    Processing = 2,
    Rejected = 3,
    Completed = 4
}
