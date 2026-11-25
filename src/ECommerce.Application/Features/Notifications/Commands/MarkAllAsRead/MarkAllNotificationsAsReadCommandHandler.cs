using ECommerce.Application.Features.Notifications.Commands.MarkAsRead;

namespace ECommerce.Application.Features.Notifications.Commands.MarkAllAsRead;

internal class MarkAllNotificationsAsReadCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ISender sender)
    : IRequestHandler<MarkAllNotificationAsReadCommand, Result>
{
    public async Task<Result> Handle(MarkAllNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var userNotificationIds = await unitOfWork
            .Repository<Notification>()
            .GetAllByPredicateProjectAsync(x => x.UserId == currentUser.Id,
            x => x.Id, cancellationToken);

        foreach (var id in userNotificationIds!)
        {
            await sender.Send(new MarkNotificationAsReadCommand(id), cancellationToken);
        }

        return Result.Success();
    }
}