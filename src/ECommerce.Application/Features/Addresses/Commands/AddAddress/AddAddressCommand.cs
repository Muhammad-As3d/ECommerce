namespace ECommerce.Application.Features.Addresses.Commands.AddAddress;

public record AddAddressCommand(
    string Street,
    string City,
    string Governorate,
    string Country,
    string PostalCode,
    string PhoneNumber,
    bool IsDefault
) : IRequest<Result>;

public class AddAddressCommandValidator : AbstractValidator<AddAddressCommand>
{
    public AddAddressCommandValidator()
    {
        RuleFor(x => x.Street).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Governorate).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).NotEmpty().Length(10, 15);
    }
}