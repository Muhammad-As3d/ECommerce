namespace ECommerce.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Street { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
    public string? ZipCode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Order> Orders { get; set; } = [];
}
