using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ProductConfiguration : AuditableEntityConfiguration<Product>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Product> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(200);

        builder.Property(c => c.Description).HasMaxLength(500);

        builder.Property(c => c.Price).HasPrecision(10, 2);

        builder.Property(x => x.Sku)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(x => x.Sku).IsUnique();
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.ToTable("Products", table =>
        {
            table.HasCheckConstraint("CK_Products_Price", "[Price] >= 0");
            table.HasCheckConstraint("CK_Products_Stock", "[Stock] >= 0 AND [ReservedStock] >= 0 AND [ReservedStock] <= [Stock]");
        });

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
