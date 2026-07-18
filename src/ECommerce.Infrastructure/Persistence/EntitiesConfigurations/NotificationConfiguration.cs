namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.Property(n => n.UserId).IsRequired().HasMaxLength(450);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(150);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(500);

        builder.HasIndex(n => new { n.UserId, n.IsRead });

        builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(n => n.UserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();
    }
}