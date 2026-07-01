namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ProductConfiguration : AuditableEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name).HasMaxLength(200);

        builder.Property(c => c.Description).HasMaxLength(500);

        builder.Property(c => c.Price).HasPrecision(10, 2);
    }
}
