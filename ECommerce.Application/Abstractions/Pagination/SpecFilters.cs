namespace ECommerce.Application.Abstractions.Pagination;

public sealed class SpecFilters
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchValue { get; init; }
    public string? SortColumn { get; init; }
    public bool IsDescending { get; init; } = false;
}

#region Validation 

public class PagingRequestValidator : AbstractValidator<SpecFilters>
{
    public PagingRequestValidator()
    {
        RuleFor(x => x.PageSize).LessThanOrEqualTo(100);
        RuleFor(x => x.PageNumber).NotEqual(0);
    }
}

#endregion