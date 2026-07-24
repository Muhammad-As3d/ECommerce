namespace ECommerce.Api.ViewModels.Addresses;

public record AddressRequest(
    string Street,
    string City,
    string Governorate,
    string Country,
    string PostalCode,
    string PhoneNumber,
    bool IsDefault
);