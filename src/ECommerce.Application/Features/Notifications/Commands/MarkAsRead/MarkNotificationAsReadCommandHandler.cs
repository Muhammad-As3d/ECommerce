namespace ECommerce.Application.Features.Notifications.Commands.MarkAsRead;

internal class MarkNotificationAsReadCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) :
    IRequestHandler<MarkNotificationAsReadCommand, Result>
{
    public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notificationIsExists = await unitOfWork
            .Repository<Notification>()
            .AnyAsync(x => x.Id == request.Id && x.UserId == currentUser.Id, cancellationToken);

        if (!notificationIsExists)
            return NotificationErrors.NotFound;

        var notification = new Notification
        {
            Id = request.Id,
            IsRead = true,
        };

        unitOfWork.Repository<Notification>().PartialUpdate(notification, ["IsRead"]);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
