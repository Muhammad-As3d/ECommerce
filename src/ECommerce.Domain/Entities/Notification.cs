using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Notification : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
