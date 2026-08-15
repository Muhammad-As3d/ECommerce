namespace ECommerce.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.CancellationReason)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(500);
    }
}
