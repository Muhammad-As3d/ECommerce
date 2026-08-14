using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Orders.Commands.OrderCheckout;

public sealed class OrderCheckoutCommandValidator : AbstractValidator<OrderCheckoutCommand>
{
    public OrderCheckoutCommandValidator()
    {
        RuleFor(x => x.ShippingAddressId)
            .NotEmpty();

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .Equal(PaymentMethod.CashOnDelivery)
            .WithMessage("Only cash on delivery is currently supported.");

        RuleFor(x => x.CouponCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.CouponCode));
    }
}
