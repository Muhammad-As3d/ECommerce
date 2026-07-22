namespace ECommerce.Application.Features.Carts.Commands.UpdateCartItem;

public record UpdateItemQuantityCommand(
    Guid CartItemId,
    int Quantity
) : IRequest<Result>;

public class UpdateItemQuantityCommandValidator : AbstractValidator<UpdateItemQuantityCommand>
{
    public UpdateItemQuantityCommandValidator()
    {
        RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
    }
}