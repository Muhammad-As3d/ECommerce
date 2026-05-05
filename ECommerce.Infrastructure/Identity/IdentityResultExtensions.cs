namespace ECommerce.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToFailureResult(this IdentityResult result)
    {
        var error = result.Errors
            .Select(e => new Error(e.Code, e.Description, ErrorType.Validation))
            .First();

        return Result.Failure(error);
    }

    //public static Result ToFailureResult<TValue>(this IdentityResult result)
    //{
    //    var error = result.Errors
    //        .Select(e => new Error(e.Code, e.Description, ErrorType.Validation))
    //        .First();

    //    return Result.Failure<TValue>(error);
    //}
}
