using ECommerce.Application.Abstractions.Constants;

namespace ECommerce.Infrastructure.Identity.Seeding;

public static class PermissionSeeder
{
    public static async Task SeedAdminPermissionsAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        var allPermissions = Permissions.GetAllPermissions()
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct()
            .ToList();

        var currentPermissions = await context.RoleClaims
            .Where(x => x.RoleId == DefaultRoles.Admin.Id && x.ClaimType == Permissions.Type)
            .Select(x => x.ClaimValue)
            .ToListAsync(cancellationToken);

        var missing = allPermissions
            .Except(currentPermissions)
            .Select(p => new IdentityRoleClaim<string>
            {
                RoleId = DefaultRoles.Admin.Id,
                ClaimType = Permissions.Type,
                ClaimValue = p
            })
            .ToList();

        if (missing.Count == 0)
            return;

        await context.RoleClaims.AddRangeAsync(missing, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}