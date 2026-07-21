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
               .WithOne(x => x.Cart)
               .HasForeignKey<Cart>(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.CartItems)
               .WithOne(x => x.Cart)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
