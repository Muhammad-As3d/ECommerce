namespace ECommerce.Domain.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public bool IsDeleted { get; set; }
}
