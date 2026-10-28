namespace ECommerce.Infrastructure.Identity.Entities;

public class User : IdentityUser
{
    public User()
    {
        Id = Guid.CreateVersion7().ToString();
        SecurityStamp = Guid.CreateVersion7().ToString();
    }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsDisabled { get; set; }

    public Cart? Cart { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
