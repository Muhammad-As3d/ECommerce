using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace ECommerce.Api.Exceptions;

internal sealed class ValidationExceptionHandler(IProblemDetailsService problemDetailsService,
    IStringLocalizer localizer) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
            return false;

        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = validationException,
            ProblemDetails = new ValidationProblemDetails(errors)
            {
                Title = localizer["Validation.Title"].Value,
                Status = StatusCodes.Status400BadRequest,
                Type = "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1"
            }
        });
    }
}
