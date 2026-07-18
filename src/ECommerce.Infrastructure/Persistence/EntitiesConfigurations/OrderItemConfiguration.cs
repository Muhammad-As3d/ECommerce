namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.Property(oi => oi.ProductNameSnapshot).IsRequired().HasMaxLength(200);
        builder.Property(oi => oi.VariantAttributesSnapshot).HasMaxLength(200);
        builder.Property(oi => oi.UnitPriceSnapshot).HasPrecision(18, 2);
    }
}