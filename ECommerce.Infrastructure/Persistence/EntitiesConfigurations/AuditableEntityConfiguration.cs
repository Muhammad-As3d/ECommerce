using ECommerce.Infrastructure.Identity.Entities;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.Property(x => x.CreatedById).HasMaxLength(150);
        //builder.Property(x => x.UpdatedById).HasMaxLength(150);
    }
}
