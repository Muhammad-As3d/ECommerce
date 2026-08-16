namespace ECommerce.Application.Features.Orders.Commands.MarkOrderAsShipped;

public sealed class MarkOrderAsShippedCommandValidator : AbstractValidator<MarkOrderAsShippedCommand>
{
    public MarkOrderAsShippedCommandValidator()
    {
        RuleFor(command => command.OrderId).NotEmpty();

        RuleFor(command => command.EstimatedDeliveryFrom).NotEmpty();
        RuleFor(command => command.EstimatedDeliveryTo)
            .NotEmpty()
            .GreaterThanOrEqualTo(command => command.EstimatedDeliveryFrom)
            .WithMessage("Estimated delivery end must not be before its start.");

        RuleFor(command => command.TrackingNumber)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.ShippingProvider)
            .NotEmpty()
            .MaximumLength(100);
    }
}
