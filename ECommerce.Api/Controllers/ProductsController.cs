using AutoMapper;
using ECommerce.Api.ViewModels.Products;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ISender sender, IMapper mapper) : ApiBaseController
{
    private readonly ISender _sender = sender;
    private readonly IMapper _mapper = mapper;

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var result = await _sender.Send(new GetAllProductsQuery());

        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return HandleCreateResult(result, "", new { });
    }
}
