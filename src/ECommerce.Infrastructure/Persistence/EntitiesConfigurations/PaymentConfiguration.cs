namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public sealed class PaymentConfiguration : AuditableEntityConfiguration<Payment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", table =>
        {
            table.HasCheckConstraint(
                "CK_Payments_Amount",
                "[Amount] > 0 AND [RefundedAmount] >= 0 " +
                "AND [RefundedAmount] <= [Amount]");
        });

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.RefundedAmount).HasPrecision(18, 2);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .IsUnicode(false);

        builder.Property(x => x.ProviderPaymentIntentId)
            .HasMaxLength(255);

        builder.Property(x => x.ProviderChargeId)
            .HasMaxLength(255);

        builder.Property(x => x.FailureCode)
            .HasMaxLength(100);

        builder.Property(x => x.FailureMessage)
            .HasMaxLength(1000);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(x => x.OrderId);

        builder.HasIndex(x => x.ProviderPaymentIntentId)
            .IsUnique()
            .HasFilter("[ProviderPaymentIntentId] IS NOT NULL");

        builder.HasIndex(x => new { x.Status, x.CreatedOn });

        builder.HasMany(x => x.Refunds)
            .WithOne(x => x.Payment)
            .HasForeignKey(x => x.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
