
namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.ListPrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Discount)
            .HasPrecision(4, 2)
            .IsRequired();
    }
}
