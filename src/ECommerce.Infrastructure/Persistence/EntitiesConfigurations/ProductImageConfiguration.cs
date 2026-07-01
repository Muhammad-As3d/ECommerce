namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ProductImageConfiguration : AuditableEntityConfiguration<ProductImage>
{
    public override void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);
    }
}
