namespace ECommerce.Api.Extensions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a Problem.");

        var localizer = httpContext.RequestServices.GetRequiredService<IStringLocalizer>();
        var localized = localizer[result.Error.Code, result.Error.Arguments];
        var detail = localized.ResourceNotFound ? result.Error.Description : localized.Value;

        var statusCode = GetStatusCode(result.Error.ErrorType);

        var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var problemDetails = factory.CreateProblemDetails(httpContext, statusCode: statusCode, detail: detail);

        if (problemDetails.Type is { } type)
        {
            problemDetails.Type = type.Replace("https://tools.ietf.org/html/rfc9110", "https://www.rfc-editor.org/rfc/rfc9110");
        }

        problemDetails.Extensions.Remove("traceId");
        problemDetails.Extensions["code"] = result.Error.Code;
        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;


        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.BadRequest => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };
}