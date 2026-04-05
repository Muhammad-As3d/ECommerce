using ECommerce.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Extensions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to ProblemDetails");

        var statusCode = GetStatusCode(result.Error.ErrorType);

        var problemDetails = new ProblemDetails
        {
            Title = result.Error.Code,
            //Detail = result.Error.Description,
            Status = statusCode,
            Type = $"https://httpstatuses.com/{statusCode}"
        };

        problemDetails.Extensions["errors"] = new[]
        {
            result.Error.Code,
            result.Error.Description
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
}
