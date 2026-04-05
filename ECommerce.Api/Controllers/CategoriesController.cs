using ECommerce.Api.Abstractions;
using ECommerce.Api.Contracts.Category;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.Features.Categories.Commands.Requests;
using ECommerce.Application.Features.Categories.Queries.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ApiControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);


        return HandleResult<GetCategoryDto, CategoryResponse>(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateCategoryCommand>(request);

        var result = await Mediator.Send(command, cancellationToken);

        return HandleCreateResult(result);
    }
}
