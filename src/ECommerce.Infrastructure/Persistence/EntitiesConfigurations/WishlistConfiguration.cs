using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class WishlistConfiguration : AuditableEntityConfiguration<Wishlist>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists");

        builder.Property(w => w.UserId).IsRequired().HasMaxLength(450);
        builder.HasIndex(w => w.UserId).IsUnique();

        builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(w => w.UserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasMany(w => w.Items)
               .WithOne(i => i.Wishlist)
               .HasForeignKey(i => i.WishlistId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
