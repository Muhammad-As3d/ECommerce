using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

internal class CartConfiguration : AuditableEntityConfiguration<Cart>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.Property(c => c.UserId)
               .IsRequired()
               .HasMaxLength(450);

        builder.HasIndex(c => c.UserId).IsUnique();

        builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasMany(c => c.CartItems)
               .WithOne()
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
