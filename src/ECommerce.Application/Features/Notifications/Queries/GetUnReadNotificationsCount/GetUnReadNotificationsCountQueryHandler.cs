namespace ECommerce.Application.Features.Notifications.Queries.GetUnReadNotificationsCount;

internal class GetUnReadNotificationsCountQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetUnReadNotificationsCountQuery, int>
{
    public async Task<int> Handle(GetUnReadNotificationsCountQuery request, CancellationToken cancellationToken)
    {
        var unReadCount = await unitOfWork
            .Repository<Notification>()
            .CountAsync(x => x.UserId == currentUser.Id && !x.IsRead, cancellationToken);

        return unReadCount;
    }
}
