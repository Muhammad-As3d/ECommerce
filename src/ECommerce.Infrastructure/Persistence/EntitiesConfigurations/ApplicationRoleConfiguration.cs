namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        var adminRole = new Role
        {
            Id = DefaultRoles.Admin.Id,
            Name = DefaultRoles.Admin.Name,
            NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
            ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp
        };

        var customerRole = new Role
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
