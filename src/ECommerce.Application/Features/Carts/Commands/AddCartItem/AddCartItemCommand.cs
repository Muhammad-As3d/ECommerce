namespace ECommerce.Application.Features.Carts.Commands.AddCartItem;

public record AddCartItemCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<Guid>>;

public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
{
    public AddCartItemCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);
    }
}