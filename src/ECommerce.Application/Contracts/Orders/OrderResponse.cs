namespace ECommerce.Application.Contracts.Orders;

public record OrderResponse(
    Guid Id,
    string OrderNumber,
    string Status,
    decimal TotalAmount,
    string Currency,
    PaymentResponse Payment);

public record PaymentResponse(
    Guid PaymentId,
    string Method,
    string Status,
    string? ClientSecret,
    decimal Amount,
    string Currency
);
