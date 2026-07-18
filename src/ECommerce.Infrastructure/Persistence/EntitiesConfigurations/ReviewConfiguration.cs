using ECommerce.Infrastructure.Persistence.EntitiesConfigurations.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.EntitiesConfigurations;

public class ReviewConfiguration : AuditableEntityConfiguration<Review>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.Property(r => r.UserId).IsRequired().HasMaxLength(450);
        builder.Property(r => r.Comment).HasMaxLength(1000);

        builder.HasIndex(r => r.OrderItemId).IsUnique();

        builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasOne(r => r.Product)
               .WithMany()
               .HasForeignKey(r => r.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.OrderItem)
               .WithMany()
               .HasForeignKey(r => r.OrderItemId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}