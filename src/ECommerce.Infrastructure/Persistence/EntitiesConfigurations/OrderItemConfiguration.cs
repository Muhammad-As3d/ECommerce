namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", table =>
        {
            table.HasCheckConstraint(
                "CK_OrderItems_Quantity",
                "[Quantity] > 0");

            table.HasCheckConstraint(
                "CK_OrderItems_Amounts",
                "[UnitPriceSnapshot] >= 0 AND " +
                "[DiscountSnapshot] >= 0 AND [TaxSnapshot] >= 0");
        });

        builder.Property(x => x.ProductNameSnapshot)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SkuSnapshot)
            .HasMaxLength(100);

        builder.Property(x => x.VariantAttributesSnapshot)
            .HasMaxLength(1000);

        builder.Property(x => x.UnitPriceSnapshot)
            .HasPrecision(18, 2);

        builder.Property(x => x.DiscountSnapshot)
            .HasPrecision(18, 2);

        builder.Property(x => x.TaxSnapshot)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.OrderId);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}