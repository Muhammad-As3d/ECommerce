namespace ECommerce.Application.Interfaces.Services;

public interface IRealTimeNotifier
{
    Task SendToUserAsync(string userId, Notification notification, CancellationToken cancellationToken = default);
}
