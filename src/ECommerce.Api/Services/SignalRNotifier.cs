using ECommerce.Api.Hubs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.Api.Services;

public class SignalRNotifier(IHubContext<NotificationHub> hubContext) : IRealTimeNotifier
{
    public Task SendToUserAsync(string userId, Notification notification, CancellationToken cancellationToken = default) =>
        hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
        {
            notification.Id,
            notification.Title,
            notification.Message,
            Type = notification.Type.ToString(),
            notification.CreatedOn
        }, cancellationToken);
}
