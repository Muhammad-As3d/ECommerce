using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class OrderConfiguration : AuditableEntityConfiguration<Order>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);
        builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(30);
        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.HasIndex(o => o.UserId);

        builder.Property(o => o.SubTotal).HasPrecision(18, 2);
        builder.Property(o => o.DiscountAmount).HasPrecision(18, 2);
        builder.Property(o => o.ShippingFee).HasPrecision(18, 2);
        builder.Property(o => o.TaxAmount).HasPrecision(18, 2);
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2);

        builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasOne(o => o.ShippingAddress)
               .WithMany()
               .HasForeignKey(o => o.ShippingAddressId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Items)
               .WithOne(oi => oi.Order)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Payment)
               .WithOne(p => p.Order)
               .HasForeignKey<Payment>(p => p.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
