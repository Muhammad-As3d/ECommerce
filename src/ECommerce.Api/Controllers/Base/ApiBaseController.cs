using ECommerce.Api.Extensions;

namespace ECommerce.Api.Controllers.Base;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected IActionResult HandleResult(Result result) => result.IsSuccess ? NoContent() : result.ToProblem(HttpContext);

    protected IActionResult HandleResult<TValue>(Result<TValue> result) =>
        result.IsSuccess ? Ok(result.Value) : result.ToProblem(HttpContext);

    protected IActionResult HandleResult<TValue>(Result<TValue> result, string routeName, Func<TValue, object> routeValues) =>
        result.IsSuccess
            ? CreatedAtAction(routeName, routeValues(result.Value), null)
            : result.ToProblem(HttpContext);
}