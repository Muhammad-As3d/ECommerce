using ECommerce.Application.Contracts.Orders;

namespace ECommerce.Application.Features.Orders.Commands;

public record OrderCheckoutCommand(
    Guid ShippingAddressId
//string ShippingMethod
) : IRequest<Result<OrderResponse>>;

public class OrderCheckoutCommandValidator : AbstractValidator<OrderCheckoutCommand>
{
    public OrderCheckoutCommandValidator()
    {
        RuleFor(x => x.ShippingAddressId).NotEmpty();
    }
}