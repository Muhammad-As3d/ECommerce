namespace ECommerce.Infrastructure.Identity.Entities;

public class Role : IdentityRole
{
    public Role()
    {
        Id = Guid.CreateVersion7().ToString();
    }

    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
