using ECommerce.Api.ViewModels.Products;
using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Commands.CreateProductImage;
using ECommerce.Application.Features.Products.Commands.DeleteProductImages;
using ECommerce.Application.Features.Products.Commands.ToggleStatus;
using ECommerce.Application.Features.Products.Commands.UpdateProduct;
using ECommerce.Application.Features.Products.Queries.GetAllProducts;
using ECommerce.Application.Features.Products.Queries.GetProduct;
using ECommerce.Infrastructure.Identity.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/v1/{categoryId:Guid}/[controller]")]
[ApiController]
public class ProductsController(ISender sender) : ApiBaseController
{
    private readonly ISender _sender = sender;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] Guid categoryId, [FromQuery] SpecificationRequest spec, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllProductsQuery(categoryId, spec), cancellationToken);

        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid categoryId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetProductByIdQuery(categoryId, id), cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromRoute] Guid categoryId, [FromForm] ProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(categoryId, request.Name, request.Description, request.Stock, request.ModelYear, request.Price, request.Images);

        var result = await _sender.Send(command, cancellationToken);

        return HandleCreatedResult(result, nameof(Get), value => new { categoryId, id = result.Value });
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPost("{productId:Guid}/images")]
    public async Task<IActionResult> CreateImages([FromRoute] Guid categoryId, [FromRoute] Guid productId, [FromForm] ProductImagesRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductImagesCommand(categoryId, productId, request.Images);

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid categoryId, [FromRoute] Guid id, [FromBody] ProductUpdateRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(categoryId, id, request.Name, request.Description, request.Stock, request.ModelYear, request.Price);

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpPut("{id:Guid}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] Guid categoryId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new ToggleStatusProductCommand(categoryId, id);

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [Authorize(Roles = DefaultRoles.Admin.Name)]
    [HttpDelete("{id:Guid}/images")]
    public async Task<IActionResult> DeleteImages([FromRoute] Guid categoryId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductImagesCommand(categoryId, id);

        var result = await _sender.Send(command, cancellationToken);

        return HandleResult(result);
    }
}
