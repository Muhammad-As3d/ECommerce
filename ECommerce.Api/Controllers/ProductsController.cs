using ECommerce.Api.ViewModels.Products;
using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.GetAllProducts;
using ECommerce.Application.Features.Products.Queries.GetProduct;
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
    public async Task<IActionResult> GetAll([FromRoute] int categoryId, [FromQuery] SpecificationRequest spec, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllProductsQuery(categoryId, spec), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get([FromRoute] int categoryId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetProductByIdQuery(categoryId, id), cancellationToken);

        return HandleResult(result);
    }

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
