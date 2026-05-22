using AutoMapper;
using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Products.Commands.Create;
using ECommerce.Application.Features.Products.Queries.GetAll;
using ECommerce.Infrastructure.Identity.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/{categoryId}/[controller]")]
[ApiController]
public class ProductsController(ISender sender, IMapper mapper) : ApiBaseController
{
    private readonly ISender _sender = sender;
    private readonly IMapper _mapper = mapper;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int categoryId, [FromRoute] SpecFilters spec, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllProductsQuery(categoryId, spec), cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int categoryId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        //var result = await _sender.Send(new GetAllProductsQuery(categoryId, spec), cancellationToken);

        return Ok();
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromRoute] int categoryId, [FromForm] CreateProductCommand command, CancellationToken cancellationToken)
    {
        //var command = new CreateProductCommand( 
        //    request.Name,
        //    request.Description,
        //    request.Stock,
        //    request.ModelYear,
        //    request.Price,
        //    categoryId,
        //    request.Images);

        command = command with { CategoryId = categoryId };

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);

        //return HandleCreatedResult(result, nameof(Get), new { id = result.Value });
    }
}
