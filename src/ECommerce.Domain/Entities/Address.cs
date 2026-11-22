using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Address : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

    private Address() { }

    public static Address CreateStub(Guid id) => new() { Id = id };

    public static Address Create(string userId, string fullName, string street, string city, string governorate, string country, string postalCode, string phoneNumber, bool isDefault)
        => new()
        {
            UserId = userId,
            FullName = fullName,
            Street = street,
            City = city,
            Governorate = governorate,
            Country = country,
            PostalCode = postalCode,
            PhoneNumber = phoneNumber,
            IsDefault = isDefault
        };

    public IReadOnlyCollection<string> Update(string street, string city, string governorate, string country, string postalCode, string phoneNumber, bool isDefault)
    {
        Street = street;
        City = city;
        Governorate = governorate;
        Country = country;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        IsDefault = isDefault;

        return [nameof(Street), nameof(City), nameof(Governorate), nameof(Country), nameof(PostalCode), nameof(PhoneNumber), nameof(IsDefault)];
    }
}
