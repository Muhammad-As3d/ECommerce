using ECommerce.Application.Abstractions.Constants;
using ECommerce.Application.Contracts.Roles;
using Mapster;

namespace ECommerce.Infrastructure.Implementations.Authentication;

internal class RoleService(RoleManager<Role> roleManager, ApplicationDbContext context) : IRoleService
{
    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default) =>
        await roleManager.Roles
            .AsNoTracking()
            .Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);

    public async Task<Result<RoleDetailResponse>> GetAsync(string roleId, CancellationToken cancellationToken)
    {
        if (await roleManager.FindByIdAsync(roleId) is not { } role)
            return RoleErrors.RoleNotFound;

        var permissions = await roleManager.GetClaimsAsync(role);

        var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(c => c.Value));

        return Result.Success(response);
    }

    public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default)
    {
        var roleIsExists = await roleManager.RoleExistsAsync(request.Name);

        if (roleIsExists)
            return RoleErrors.DuplicatedRole;

        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(allowedPermissions).Any())
            return RoleErrors.InvalidPermissions;

        var role = new Role
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return result.ToError();

        var permissions = request.Permissions
            .Select(x => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = x,
                RoleId = role.Id,
            });

        await context.AddRangeAsync(permissions, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var response = new RoleDetailResponse(role.Id, role.Name, role.IsDeleted, request.Permissions);

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string Id, RoleRequest request, CancellationToken cancellationToken = default)
    {
        if (await roleManager.FindByIdAsync(Id) is not { } role)
            return RoleErrors.RoleNotFound;

        if (role.IsDefault)
            return RoleErrors.DefaultRoleCannotBeModified;

        if (role.Name == DefaultRoles.Admin.Name)
            return RoleErrors.AdminRoleCannotBeModified;

        var roleIsExists = await roleManager.Roles.AnyAsync(x => x.Name == request.Name && x.Id != Id, cancellationToken);

        if (roleIsExists)
            return RoleErrors.DuplicatedRole;

        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(allowedPermissions).Any())
            return RoleErrors.InvalidPermissions;

        role.Name = request.Name;
        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            return result.ToError();

        var currentPermissions = await context.RoleClaims
            .Where(x => x.RoleId == Id && x.ClaimType == Permissions.Type)
            .Select(x => x.ClaimValue)
            .ToListAsync(cancellationToken);

        var newPermissions = request.Permissions.Except(currentPermissions)
            .Select(x => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = x,
                RoleId = role.Id
            });

        var removedPermissions = currentPermissions.Except(request.Permissions);

        await context.RoleClaims
            .Where(x => x.RoleId == Id && removedPermissions.Contains(x.ClaimValue))
            .ExecuteDeleteAsync(cancellationToken);

        await context.AddRangeAsync(newPermissions, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await roleManager.FindByIdAsync(id) is not { } role)
            return RoleErrors.RoleNotFound;

        if (role.IsDefault)
            return RoleErrors.DefaultRoleCannotBeModified;

        if (role.Name == DefaultRoles.Admin.Name)
            return RoleErrors.AdminRoleCannotBeModified;

        role.IsDeleted = !role.IsDeleted;
        var result = await roleManager.UpdateAsync(role);

        return result.Succeeded ? Result.Success() : result.ToError();
    }

    public async Task<List<string>?> GetAllPermissionsAsync(CancellationToken cancellationToken = default) =>
        Permissions.GetAllPermissions().Distinct().ToList()!;
}