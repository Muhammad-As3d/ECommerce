namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.Property(ci => ci.UnitPriceSnapshot).HasPrecision(18, 2);

        builder.HasIndex(ci => ci.CartId).IsUnique();

    }
}