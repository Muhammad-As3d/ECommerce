namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(255);

        builder.Property(c => c.Description).HasMaxLength(300);

        builder.Property(c => c.Price).HasPrecision(10, 2);
    }
}
