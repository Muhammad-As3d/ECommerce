using ECommerce.Api.ViewModels.Products;
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
public class ProductsController(ISender sender) : ApiBaseController
{
    private readonly ISender _sender = sender;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int categoryId, [FromRoute] SpecificationRequest spec, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllProductsQuery(categoryId, spec), cancellationToken);

        return HandleResult(result);
    }

    //[HttpGet("{id}")]
    //public async Task<IActionResult> Get([FromRoute] int categoryId, [FromRoute] int id, CancellationToken cancellationToken)
    //{
    //    //var result = await _sender.Send(new GetAllProductsQuery(categoryId, spec), cancellationToken);

    //    return Ok();
    //}

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromRoute] int categoryId, [FromForm] ProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(categoryId, request.Name, request.Description, request.Stock, request.ModelYear, request.Price, request.Images);

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);

        //return HandleCreatedResult(result, nameof(Get), new { id = result.Value });
    }
}
