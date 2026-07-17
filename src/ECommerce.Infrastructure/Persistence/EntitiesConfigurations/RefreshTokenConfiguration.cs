namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(1000);
    }
}
