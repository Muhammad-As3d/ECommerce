using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ProductConfiguration : AuditableEntityConfiguration<Product>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Product> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(200);

        builder.Property(c => c.Description).HasMaxLength(500);

        builder.Property(c => c.Price).HasPrecision(10, 2);
    }
}