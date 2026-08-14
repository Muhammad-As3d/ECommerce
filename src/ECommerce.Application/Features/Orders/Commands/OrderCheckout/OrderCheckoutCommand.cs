using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Orders.Commands.OrderCheckout;

public record OrderCheckoutCommand(
    Guid ShippingAddressId,
    PaymentMethod PaymentMethod,
    string? CouponCode = null
) : IRequest<Result<OrderResponse>>;
