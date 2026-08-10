using ECommerce.Domain.Enums;

namespace ECommerce.Application.Abstractions.Pagination;

public sealed class FiltersRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchValue { get; init; }
    public string? SortColumn { get; init; }
    public bool IsDescending { get; init; } = false;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public string? OrderNumber { get; set; }
}

#region Validation 

public class SpecificationRequestValidator : AbstractValidator<FiltersRequest>
{
    public SpecificationRequestValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(1)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.PageNumber)
            .NotEqual(0);
    }
}

#endregion