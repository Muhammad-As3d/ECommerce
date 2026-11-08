namespace ECommerce.Infrastructure.Extensions;

public static class UserExtensions
{
    public static Error ToError(this IdentityResult result)
    {
        var error = result.Errors.First();
        return new Error(error.Code, error.Description, ErrorType.BadRequest);
    }
}
