namespace ECommerce.Application.Contracts.Orders;

public record OrderUserResponse(
    Guid Id,
    string OrderNumber,
    string Status,
    DateTime CreatedOn,
    int ItemCount,
    int WithinDays,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal ShippingFee,
    decimal TaxAmount,
    decimal TotalAmount
);