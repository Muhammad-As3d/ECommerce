using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Features.Products.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());

        return Ok(result);
    }
}
