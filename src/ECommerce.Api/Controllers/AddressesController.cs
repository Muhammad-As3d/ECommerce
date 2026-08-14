using ECommerce.Api.ViewModels.Addresses;
using ECommerce.Application.Features.Addresses.Commands.AddAddress;
using ECommerce.Application.Features.Addresses.Commands.DeleteAddress;
using ECommerce.Application.Features.Addresses.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class AddressesController(ISender sender) : ApiBaseController
{
    [HttpGet("")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAddressQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(AddressRequest request, CancellationToken cancellationToken = default)
    {
        await sender.Send(request.Adapt<AddAddressCommand>(), cancellationToken);

        return Created();
    }

    [HttpDelete("")]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteAddressCommand(), cancellationToken);

        return HandleResult(result);
    }
}
