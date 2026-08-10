using System.Linq.Expressions;

namespace ECommerce.Application.Specifications.OrderSpecifications;

internal class AllOrdersSpecification : Specification<Order>
{
    public AllOrdersSpecification(FiltersRequest filters)
    {
        var searchValue = filters.SearchValue?.Trim();
        var orderNumber = filters.OrderNumber?.Trim();

        Predicate = x =>
            (string.IsNullOrWhiteSpace(searchValue) || x.Id.ToString().Contains(searchValue)) &&
            (string.IsNullOrWhiteSpace(orderNumber) || x.OrderNumber.Contains(orderNumber)) &&
            (!filters.OrderStatus.HasValue || x.Status == filters.OrderStatus.Value) &&
            (!filters.StartDate.HasValue || x.CreatedOn >= filters.StartDate.Value) &&
            (!filters.EndDate.HasValue || x.CreatedOn < filters.EndDate.Value.Date.AddDays(1));

        ApplySorting(filters);
    }

    private void ApplySorting(FiltersRequest spec)
    {
        Action<Expression<Func<Order, object>>> sort = spec.IsDescending ? SortingByDescending : SortingBy;

        switch (spec.SortColumn?.ToLower())
        {
            case "createdon":
                sort(x => x.CreatedOn);
                break;
            case "totalamount":
                sort(x => x.TotalAmount);
                break;
            default:
                sort(x => x.Id);
                break;
        }
    }
}