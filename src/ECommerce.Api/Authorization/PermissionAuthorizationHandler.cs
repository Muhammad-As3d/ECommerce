namespace ECommerce.Api.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = context.User.Identity;

        var hasPermission = context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type);

        if (user is null || !user.IsAuthenticated || !hasPermission)
            return;

        context.Succeed(requirement);
        return;
    }
}
