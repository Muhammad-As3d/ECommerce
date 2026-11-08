namespace ECommerce.Api.Authorization;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{
}