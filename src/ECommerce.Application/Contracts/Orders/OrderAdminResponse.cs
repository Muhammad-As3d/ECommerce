namespace ECommerce.Application.Contracts.Orders;

public record OrderAdminResponse(
     Guid Id,
    string OrderNumber,
    string Status,
    string PaymentStatus,
    DateTime CreatedOn,
    int WithinDays,
    decimal SubTotal,
    decimal TotalAmount,
    string UserId
);