namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public sealed class RefundConfiguration : AuditableEntityConfiguration<Refund>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("Refunds", table =>
        {
            table.HasCheckConstraint("CK_Refunds_Amount", "[Amount] > 0");
        });

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(3).IsUnicode(false);
        builder.Property(x => x.Reason).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ProviderRefundId).HasMaxLength(255);
        builder.Property(x => x.FailureMessage).HasMaxLength(1000);

        builder.HasIndex(x => x.PaymentId);
        builder.HasIndex(x => x.ProviderRefundId)
            .IsUnique()
            .HasFilter("[ProviderRefundId] IS NOT NULL");
    }
}
