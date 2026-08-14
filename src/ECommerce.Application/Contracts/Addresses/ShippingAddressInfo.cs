namespace ECommerce.Application.Contracts.Addresses;

public sealed record ShippingAddressInfo(
    string FullName,
    string PhoneNumber,
    string Street,
    string City,
    string Governorate,
    string Country,
    string? PostalCode);
