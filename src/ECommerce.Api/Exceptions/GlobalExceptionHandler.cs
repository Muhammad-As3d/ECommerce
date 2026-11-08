using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace ECommerce.Api.Exceptions;

internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(exception, "An unexpected error occurred. TraceId: {TraceId}", traceId);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = "https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1",
                Title = "An unexpected error occurred.",
                Status = StatusCodes.Status500InternalServerError,
                Extensions = { ["traceId"] = traceId }
            }
        });
    }
}