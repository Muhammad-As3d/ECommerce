using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Domain.Entities.Common;

public class AuditableEntity
{
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string? UpdatedById { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser? UpdatedBy { get; set; } = default!;
}
