namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);

        var adminUser = new ApplicationUser
        {
            Id = DefaultUsers.Admin.Id,
            FirstName = DefaultUsers.Admin.FirstName,
            LastName = DefaultUsers.Admin.LastName,
            Email = DefaultUsers.Admin.Email,
            NormalizedEmail = DefaultUsers.Admin.Email.ToUpper(),
            UserName = DefaultUsers.Admin.Email,
            NormalizedUserName = DefaultUsers.Admin.Email.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = DefaultUsers.Admin.SecurityStamp,
            ConcurrencyStamp = DefaultUsers.Admin.ConcurrencyStamp,
            PasswordHash = DefaultUsers.Admin.PasswordHash,
        };

        builder.HasData(adminUser);
    }
}
