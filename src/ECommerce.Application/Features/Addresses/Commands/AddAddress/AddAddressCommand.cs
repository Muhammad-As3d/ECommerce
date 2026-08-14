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