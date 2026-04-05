using AutoMapper;
using ECommerce.Api.Extensions;
using ECommerce.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Abstractions;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase : ControllerBase
{
    private IMediator? _mediatorInstance;
    private IMapper? _mapperInstance;

    protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper => _mapperInstance ??= HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected IActionResult HandleResult(Result result) => result.IsSuccess ? NoContent() : result.ToProblem();

    protected IActionResult HandleResult<TValue, TViewModel>(Result<TValue> result) =>
        result.IsSuccess ? Ok(Mapper.Map<TViewModel>(result.Value)) : result.ToProblem();

    protected IActionResult HandleCreateResult(Result result) =>
        result.IsSuccess ? Created() : result.ToProblem();
}
