using ECommerce.Application.Contracts.Payments;
using ECommerce.Application.Features.Payments.Commands.CreatePayment;
using ECommerce.Application.Features.Payments.Commands.PaymobWebhook;

namespace ECommerce.Api.Controllers;

[Route("api/v1/payments")]
public class PaymentsController(ISender sender) : ApiBaseController
{
    [Authorize(Roles = DefaultRoles.Customer.Name)]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("paymob/webhook")]
    public async Task<IActionResult> PaymobWebhook([FromBody] PaymobWebhookRequest request, [FromQuery] string hmac, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new PaymobWebhookCommand(request.Obj, hmac), cancellationToken);

        return HandleResult(result);
    }
}
