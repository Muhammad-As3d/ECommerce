using ECommerce.Application.Abstractions.Results;
using ECommerce.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Extensions;

public static class ResultExtension
{
    //public static ObjectResult ToProblem(this Result result)
    //{
    //    if (result.IsSuccess)
    //        throw new InvalidOperationException("Cannot convert success result to a problem");

    //    var problem = Results.Problem();

    //    var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

    //    problemDetails!.Extensions = new Dictionary<string, object?>
    //    {
    //        {
    //            "errors", new[]
    //            {
    //                result.Error.Code,
    //                result.Error.Description
    //            }
    //        }
    //    };

    //    return new ObjectResult(problemDetails);
    //}

    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to ProblemDetails");

        var problemDetails = new ProblemDetails
        {
            Title = result.Error.Code,
            //Detail = result.Error.Description,
            //Status = GetStatusCode(result.Error.ErrorType),
            Type = $"https://httpstatuses.com/{GetStatusCode(result.Error.ErrorType)}"
        };

        problemDetails.Extensions["errors"] = new[]
        {
            result.Error.Code,
            result.Error.Description
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
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
