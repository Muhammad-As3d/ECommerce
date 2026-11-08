using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class RoleErrors
{
    public static readonly Error RoleNotFound =
        Error.NotFound("Role.RoleNotFound", "this role is not found.");
    public static readonly Error DuplicatedRole =
        Error.Conflict("Role.DuplicatedRole", "the same role name is found.");
    public static readonly Error InvalidPermissions =
        Error.BadRequest("Role.InvalidPermissions", "this permissions is invalid.");
    public static readonly Error DefaultRoleCannotBeModified =
    Error.BadRequest("Role.DefaultRoleCannotBeModified", "default roles cannot be modified.");
}
