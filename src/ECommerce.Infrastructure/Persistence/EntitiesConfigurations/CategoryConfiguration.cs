using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class CategoryConfiguration : AuditableEntityConfiguration<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
    }
}
