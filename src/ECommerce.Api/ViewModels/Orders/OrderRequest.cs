using ECommerce.Domain.Enums;

namespace ECommerce.Api.ViewModels.Orders;

public record OrderRequest(
    Guid ShippingAddressId,
    PaymentMethod PaymentMethod,
    string? CouponCode = null
);