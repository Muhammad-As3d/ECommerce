using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class PaymentConfiguration : AuditableEntityConfiguration<Payment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.Property(p => p.Provider).IsRequired().HasMaxLength(50);
        builder.Property(p => p.ProviderTransactionId).HasMaxLength(100);
        builder.Property(p => p.Amount).HasPrecision(18, 2);

        builder.HasIndex(p => p.OrderId).IsUnique();
    }
}