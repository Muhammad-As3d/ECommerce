namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(255);
        builder.Property(c => c.ListPrice).HasPrecision(10, 2);
    }
}
