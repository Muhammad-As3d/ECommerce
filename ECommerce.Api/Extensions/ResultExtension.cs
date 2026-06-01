using ECommerce.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Extensions;

public static class ResultExtension
{
    public static IActionResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to ProblemDetails.");

        var statusCode = GetStatusCode(result.Error.ErrorType);


        var problem = Results.Problem(statusCode: statusCode);

        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;


        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors" , new[] { result.Error.Code, result.Error.Description }
            }
        };

        return new ObjectResult(problemDetails);

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