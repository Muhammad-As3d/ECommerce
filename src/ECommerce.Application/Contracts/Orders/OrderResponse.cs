using ECommerce.Application.Contracts.Addresses;
using ECommerce.Application.Contracts.Carts;

namespace ECommerce.Application.Contracts.Orders;

public record OrderResponse(
    Guid Id,
    string OrderNumber,
    string Status,
    DateTime CreatedOn,
    int WithinDays,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal ShippingFee,
    decimal TaxAmount,
    decimal TotalAmount,
    AddressResponse ShippingAddress,
    IEnumerable<CartItemResponse> Items
);