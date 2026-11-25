namespace ECommerce.Application.Contracts.Notifications;

public record NotificationResponse(
    Guid Id,
    string UserId,
    string Title,
    string Message,
    NotificationType Type,
    bool IsRead,
    DateTime CreatedOn
);
