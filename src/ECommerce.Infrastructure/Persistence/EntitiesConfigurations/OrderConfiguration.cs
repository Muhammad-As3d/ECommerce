namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public sealed class OrderConfiguration : AuditableEntityConfiguration<Order>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", table =>
        {
            table.HasCheckConstraint(
                "CK_Orders_Amounts",
                "[SubTotal] >= 0 AND [DiscountAmount] >= 0 " +
                "AND [ShippingFee] >= 0 AND [TaxAmount] >= 0 " +
                "AND [TotalAmount] >= 0");
        });

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .IsUnicode(false);

        builder.Property(x => x.SubTotal).HasPrecision(18, 2);
        builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);
        builder.Property(x => x.ShippingFee).HasPrecision(18, 2);
        builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.CancellationReason).HasMaxLength(500);
        builder.Property(x => x.TrackingNumber).HasMaxLength(100);
        builder.Property(x => x.ShippingProvider).HasMaxLength(100);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(x => x.OrderNumber)
            .IsUnique();

        builder.HasIndex(x => new { x.UserId, x.CreatedOn });

        builder.HasIndex(x => new { x.Status, x.CreatedOn });

        builder.OwnsOne(x => x.ShippingAddress, address =>
        {
            address.Property(x => x.FullName)
                .HasColumnName("ShippingFullName")
                .HasMaxLength(200)
                .IsRequired();

            address.Property(x => x.PhoneNumber)
                .HasColumnName("ShippingPhoneNumber")
                .HasMaxLength(20)
                .IsRequired();

            address.Property(x => x.Street)
                .HasColumnName("ShippingStreet")
                .HasMaxLength(250)
                .IsRequired();

            address.Property(x => x.City)
                .HasColumnName("ShippingCity")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.Governorate)
                .HasColumnName("ShippingGovernorate")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.Country)
                .HasColumnName("ShippingCountry")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.PostalCode)
                .HasColumnName("ShippingPostalCode")
                .HasMaxLength(20);
        });

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Payments)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.StatusHistory)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
