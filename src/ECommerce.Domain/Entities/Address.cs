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
}
