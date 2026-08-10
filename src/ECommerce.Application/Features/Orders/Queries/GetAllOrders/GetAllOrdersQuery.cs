using ECommerce.Application.Specifications.OrderSpecifications;

namespace ECommerce.Application.Features.Orders.Queries.GetAllOrders;

public record GetAllOrdersQuery(FiltersRequest Filters) : IRequest<PaginatedList<OrderAdminResponse>>;

public class GetAllOrdersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllOrdersQuery, PaginatedList<OrderAdminResponse>>
{
    public async Task<PaginatedList<OrderAdminResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var Spec = new AllOrdersSpecification(request.Filters);

        return await unitOfWork
            .Repository<Order>()
            .GetAllPaginatedProjectAsync<OrderAdminResponse>(Spec, request.Filters.PageNumber, request.Filters.PageSize, cancellationToken);
    }
}