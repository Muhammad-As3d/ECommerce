namespace ECommerce.Application.Contracts.Orders;

public record OrderDetailsResponse(
    Guid Id,
    string OrderNumber,
    string Status,
    DateTime CreatedOn,
    string PaymentMethod,
    string Currency,
    DateTimeOffset? EstimatedDeliveryFrom,
    DateTimeOffset? EstimatedDeliveryTo,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal ShippingFee,
    decimal TaxAmount,
    decimal TotalAmount,
    ShippingAddressResponse ShippingAddress,
    IEnumerable<OrderItemResponse> Items
);

public record ShippingAddressResponse(
    string FullName,
    string Street,
    string City,
    string Governorate,
    string Country,
    string? PostalCode,
    string PhoneNumber
);

public record OrderItemResponse(
    Guid Id,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice
);
