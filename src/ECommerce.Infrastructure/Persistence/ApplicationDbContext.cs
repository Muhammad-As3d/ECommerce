using System.Reflection;

namespace ECommerce.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<ProductImage> ProductImages { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderItem> OrderItems { get; set; } = default!;
    public DbSet<Cart> Carts { get; set; } = default!;
    public DbSet<CartItem> CartItems { get; set; } = default!;
    public DbSet<Notification> Notifications { get; set; } = default!;
    public DbSet<Payment> Payments { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Wishlist> Wishlists { get; set; } = default!;
    public DbSet<WishlistItem> WishlistItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFks = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

        foreach (var fk in cascadeFks)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entity in entries)
        {
            if (entity.State == EntityState.Added)
                entity.Property(c => c.CreatedById).CurrentValue = currentUserId!;

            else if (entity.State == EntityState.Modified)
            {
                entity.Property(c => c.UpdatedById).CurrentValue = currentUserId;
                entity.Property(c => c.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
