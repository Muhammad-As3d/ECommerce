using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class NotificationErrors
{
    public static Error NotFound =>
   Error.NotFound("Notifications.NotFound", "this notification with ID is not found.");
}
