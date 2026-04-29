using AutoMapper;
using ECommerce.Api.Extensions;
using ECommerce.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Abstractions;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiControllerBase(IMediator mediator, IMapper mapper) : ControllerBase
{
    protected IMediator _mediator { get; } = mediator;
    protected IMapper _mapper { get; } = mapper;
    protected IActionResult HandleResult(Result result) =>
        result.IsSuccess ? NoContent() : result.ToProblem();

    protected IActionResult HandleResult<TValue, TViewModel>(Result<TValue> result) =>
        result.IsSuccess ? Ok(_mapper.Map<TViewModel>(result.Value))
        : result.ToProblem();

    protected IActionResult HandleResult<TValue, TViewModel>(Result<IEnumerable<TValue>> result) =>
    result.IsSuccess
        ? Ok(_mapper.Map<IEnumerable<TViewModel>>(result.Value))
        : result.ToProblem();

    protected IActionResult HandleCreateResult<TValue>(Result<TValue> result, string routeName, object routeValue) =>
        result.IsSuccess ? CreatedAtAction(routeName, routeValue, result.Value)
        : result.ToProblem();
}
