namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.FirstName).HasMaxLength(255);
        builder.Property(c => c.LastName).HasMaxLength(255);
        builder.Property(c => c.Email).HasMaxLength(255);
        builder.Property(c => c.Street).HasMaxLength(255);
        builder.Property(c => c.Phone).HasMaxLength(25);
        builder.Property(c => c.City).HasMaxLength(50);
        builder.Property(c => c.State).HasMaxLength(25);
        builder.Property(c => c.ZipCode).HasMaxLength(5);
    }
}
