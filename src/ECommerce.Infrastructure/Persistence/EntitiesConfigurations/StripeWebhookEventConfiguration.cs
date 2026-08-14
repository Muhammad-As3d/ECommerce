namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public sealed class StripeWebhookEventConfiguration : IEntityTypeConfiguration<StripeWebhookEvent>
{
    public void Configure(EntityTypeBuilder<StripeWebhookEvent> builder)
    {
        builder.ToTable("StripeWebhookEvents");

        builder.Property(x => x.StripeEventId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Payload)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.ProcessingError)
            .HasMaxLength(2000);

        builder.HasIndex(x => x.StripeEventId)
            .IsUnique();

        builder.HasIndex(x => new { x.ProcessedAt, x.ReceivedAt });
    }
}