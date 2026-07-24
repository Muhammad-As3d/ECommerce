using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Address : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

    private Address() { }

    public static Address Create(string userId, string street, string city, string governorate, string country, string postalCode, string phoneNumber, bool isDefault)
        => new()
        {
            UserId = userId,
            Street = street,
            City = city,
            Governorate = governorate,
            Country = country,
            PostalCode = postalCode,
            PhoneNumber = phoneNumber,
            IsDefault = isDefault
        };
}
