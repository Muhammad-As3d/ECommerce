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
        var result = await sender.Send(request.Adapt<AddAddressCommand>(), cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("")]
    public async Task<IActionResult> Update(AddressRequest request, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(request.Adapt<UpdateAddressCommand>(), cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("")]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new DeleteAddressCommand(), cancellationToken);

        return HandleResult(result);
    }
}
