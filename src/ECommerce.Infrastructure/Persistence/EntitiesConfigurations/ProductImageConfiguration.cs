using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ProductImageConfiguration : AuditableEntityConfiguration<ProductImage>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);
    }
}
