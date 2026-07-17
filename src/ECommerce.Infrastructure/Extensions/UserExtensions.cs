namespace ECommerce.Infrastructure.Extensions;

public static class UserExtensions
{
    public static Result ToFailureIdentityResult(this IdentityResult result)
    {
        var error = result.Errors
            .Select(e => new Error(e.Code, e.Description, ErrorType.BadRequest))
            .First();

        return Result.Failure(error);
    }
}
