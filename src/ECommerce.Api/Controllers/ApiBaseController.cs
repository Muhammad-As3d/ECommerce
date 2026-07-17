using ECommerce.Api.Extensions;
using ECommerce.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected IActionResult HandleResult(Result result) =>
        result.IsSuccess ? NoContent() : result.ToProblem();

    protected IActionResult HandleResult<TValue>(Result<TValue> result) =>
        result.IsSuccess
        ? Ok(result.Value)
        : result.ToProblem();

    protected IActionResult HandleCreatedResult<TValue>(Result<TValue> result, string routeName, Func<TValue, object> routeValues) =>
        result.IsSuccess
            ? CreatedAtAction(routeName, routeValues(result.Value), null)
            : result.ToProblem();
}
