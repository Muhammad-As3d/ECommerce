using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Products.Create;
using ECommerce.Application.Features.Products.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/{categoryId}/[controller]")]
[ApiController]
public class ProductsController(ISender sender) : ApiBaseController
{
    private readonly ISender _sender = sender;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int categoryId, [FromRoute] SpecFilters spec, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllProductsQuery(categoryId, spec), cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return HandleCreateResult(result, "", new { });
    }
}
