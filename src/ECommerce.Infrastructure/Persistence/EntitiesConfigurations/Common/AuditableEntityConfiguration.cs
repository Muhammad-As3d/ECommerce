namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;

public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AuditableEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedById)
              .IsRequired()
              .HasMaxLength(450);

        builder.Property(e => e.UpdatedById)
              .HasMaxLength(450);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);

        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}
