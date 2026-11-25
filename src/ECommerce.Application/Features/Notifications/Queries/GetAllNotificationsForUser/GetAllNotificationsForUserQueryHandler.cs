using ECommerce.Application.Contracts.Notifications;
using ECommerce.Application.Specifications.Notifications;

namespace ECommerce.Application.Features.Notifications.Queries.GetAllNotificationsForUser;

internal class GetAllNotificationsForUserQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetAllNotificationsForUserQuery, PaginatedList<NotificationResponse>>
{
    public async Task<PaginatedList<NotificationResponse>> Handle(GetAllNotificationsForUserQuery request, CancellationToken cancellationToken)
    {
        var spec = new NotificationSpecification(currentUser.Id);

        var notifications = await unitOfWork
            .Repository<Notification>()
            .GetAllPaginatedProjectSpecAsync<NotificationResponse>(spec, request.filter.PageNumber, request.filter.PageSize, cancellationToken);

        return notifications;
    }
}
