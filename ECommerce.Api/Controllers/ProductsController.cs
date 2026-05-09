using ECommerce.Application.Features.Products.Create;
using ECommerce.Application.Features.Products.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ISender sender) : ApiBaseController
{
    private readonly ISender _sender = sender;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllProductsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return HandleCreateResult(result, "", new { });
    }
}
