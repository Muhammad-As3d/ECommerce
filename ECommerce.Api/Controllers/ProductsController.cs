using ECommerce.Api.Abstractions;
using ECommerce.Api.Contracts.Products;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ApiControllerBase
{

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var result = await Mediator.Send(new GetAllProductsQuery());

        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(ProductRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateProductCommand>(request);

        var result = await Mediator.Send(command, cancellationToken);

        return HandleCreateResult(result);
    }
}
