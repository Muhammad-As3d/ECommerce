using AutoMapper;
using ECommerce.Api.Abstractions;
using ECommerce.Api.ViewModels.Products;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ApiControllerBase
{
    public ProductsController(IMediator mediator, IMapper mapper)
    : base(mediator, mapper) { }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());

        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return HandleCreateResult(result, "", new { });
    }
}
