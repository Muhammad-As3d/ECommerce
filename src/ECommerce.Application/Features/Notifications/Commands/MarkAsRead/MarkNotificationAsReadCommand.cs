namespace ECommerce.Application.Features.Notifications.Commands.MarkAsRead;

public record MarkNotificationAsReadCommand(Guid Id) : IRequest<Result>;
