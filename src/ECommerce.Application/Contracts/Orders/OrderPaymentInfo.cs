namespace ECommerce.Application.Contracts.Orders;

public record OrderPaymentInfo(
    Guid Id,
    OrderStatus Status,
    PaymentMethod PaymentMethod,
    decimal TotalAmount,
    string Currency,
    string FullName,
    string PhoneNumber,
    string Street,
    string City,
    string Governorate,
    string Country
);
