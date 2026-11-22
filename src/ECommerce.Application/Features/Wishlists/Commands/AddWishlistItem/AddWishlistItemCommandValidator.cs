namespace ECommerce.Application.Features.Wishlists.Commands.AddWishlistItem;

public class AddWishlistItemCommandValidator : AbstractValidator<AddWishlistItemCommand>
{
    public AddWishlistItemCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
    }
}
