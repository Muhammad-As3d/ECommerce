namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
{
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        builder.ToTable("WishlistItems");

        builder.HasIndex(i => new { i.WishlistId, i.ProductId }).IsUnique();

        builder.HasOne(i => i.Product)
               .WithMany()
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}