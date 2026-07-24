namespace ECommerce.Application.Contracts.Addresses;

public record AddressResponse(
    Guid Id,
    string Street,
    string City,
    string Governorate,
    string Country,
    string PostalCode,
    string PhoneNumber,
    bool IsDefault
);

public record AddressUserResponse(
    string FullName,
    AddressResponse AddressResponse
);