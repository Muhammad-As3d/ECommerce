using ECommerce.Infrastructure.Identity.Seeding;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        var adminRole = new ApplicationRole
        {
            Id = DefaultRoles.Admin.Id,
            Name = DefaultRoles.Admin.Name,
            NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
            ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp
        };

        var customerRole = new ApplicationRole
        {
            Id = DefaultRoles.Customer.Id,
            Name = DefaultRoles.Customer.Name,
            NormalizedName = DefaultRoles.Customer.Name.ToUpper(),
            ConcurrencyStamp = DefaultRoles.Customer.ConcurrencyStamp,
            IsDefault = true
        };

        builder.HasData(adminRole, customerRole);
    }
}
