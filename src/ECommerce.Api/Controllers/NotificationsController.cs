using ECommerce.Application.Features.Notifications.Commands.MarkAllAsRead;
using ECommerce.Application.Features.Notifications.Commands.MarkAsRead;
using ECommerce.Application.Features.Notifications.Queries.GetAllNotificationsForUser;
using ECommerce.Application.Features.Notifications.Queries.GetUnReadNotificationsCount;

namespace ECommerce.Api.Controllers;

[Authorize(Roles = DefaultRoles.Customer.Name)]
[Route("api/v1/[controller]")]
public class NotificationsController(ISender sender) : ApiBaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllForUser([FromQuery] FiltersRequest filters, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetAllNotificationsForUserQuery(filters), cancellationToken);

        return Ok(response);
    }

    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new MarkNotificationAsReadCommand(id), cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new MarkAllNotificationAsReadCommand(), cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnReadCount(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetUnReadNotificationsCountQuery(), cancellationToken);

        return Ok(response);
    }
}
