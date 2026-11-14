namespace ECommerce.Application.Contracts.Users;

public sealed record UserFiltersRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchValue { get; init; }
    public string? SortColumn { get; init; }
    public bool IsDescending { get; init; }
    public string? Role { get; init; }
    public bool? IsDisabled { get; init; }
    public bool? DefaultRole { get; init; } 
}

public class UserFiltersRequestValidator : AbstractValidator<UserFiltersRequest>
{
    public UserFiltersRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.SearchValue).MaximumLength(100);
    }
}