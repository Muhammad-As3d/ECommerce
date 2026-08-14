namespace ECommerce.Application.Contracts.Orders;

public record OrderAdminResponse(
     Guid Id,
    string OrderNumber,
    string CustomerName,
    string Status,
    string PaymentStatus,
    DateTime CreatedOn,
    string PaymentMethod,
    decimal SubTotal,
    decimal TotalAmount,
    string UserId
);
