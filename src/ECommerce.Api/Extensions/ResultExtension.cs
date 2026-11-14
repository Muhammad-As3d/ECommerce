namespace ECommerce.Api.Extensions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a Problem.");

        var error = result.Error;
        var services = httpContext.RequestServices;

        var localized = services.GetRequiredService<IStringLocalizer>()[error.Code, error.Arguments];
        var detail = localized.ResourceNotFound ? error.Description : localized.Value;
        var statusCode = GetStatusCode(error.ErrorType);

        var problem = services.GetRequiredService<ProblemDetailsFactory>()
            .CreateProblemDetails(httpContext, statusCode: statusCode, detail: detail);

        problem.Extensions["code"] = error.Code;

        return new ObjectResult(problem) { StatusCode = statusCode };
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.BadRequest => StatusCodes.Status400BadRequest,
        ErrorType.Failure => StatusCodes.Status422UnprocessableEntity,
        _ => StatusCodes.Status500InternalServerError
    };
}