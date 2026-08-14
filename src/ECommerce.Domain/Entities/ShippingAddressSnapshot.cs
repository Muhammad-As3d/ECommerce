namespace ECommerce.Domain.Entities;

public sealed class ShippingAddressSnapshot
{
    public string FullName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Governorate { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string? PostalCode { get; private set; }

    private ShippingAddressSnapshot() { }

    public static ShippingAddressSnapshot Create(string fullName, string phoneNumber, string street, string city,
        string governorate, string country, string? postalCode) =>
         new()
         {
             FullName = fullName.Trim(),
             PhoneNumber = phoneNumber.Trim(),
             Street = street.Trim(),
             City = city.Trim(),
             Governorate = governorate.Trim(),
             Country = country.Trim(),
             PostalCode = postalCode?.Trim()
         };

}