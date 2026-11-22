namespace ECommerce.Application.Features.Addresses.Commands.UpdateAddress;

public record UpdateAddressCommand(
    string Street,
    string City,
    string Governorate,
    string Country,
    string PostalCode,
    string PhoneNumber,
    bool IsDefault
) : IRequest<Result>;
