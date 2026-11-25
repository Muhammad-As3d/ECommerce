using ECommerce.Application.Contracts.Notifications;

namespace ECommerce.Application.Features.Notifications.Queries.GetAllNotificationsForUser;

public record GetAllNotificationsForUserQuery(FiltersRequest filter)
    : IRequest<PaginatedList<NotificationResponse>>;
