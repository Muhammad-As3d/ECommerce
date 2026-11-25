namespace ECommerce.Application.Specifications.Notifications;

public class NotificationSpecification : Specification<Notification>
{
    public NotificationSpecification(string UserId)
    {
        Predicate = x => x.UserId == UserId;

        SortingBy(x => x.IsRead);
    }
}
