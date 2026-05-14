namespace ECommerce.Application.Abstractions.Pagination;

public sealed class PageFilters
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchValue { get; init; }
    public bool IsDescending { get; init; }
}

#region Validation 

public class PagingRequestValidator : AbstractValidator<PageFilters>
{
    public PagingRequestValidator()
    {
        RuleFor(x => x.PageSize).LessThanOrEqualTo(100);
        RuleFor(x => x.PageNumber).NotEqual(0);
    }
}

#endregion