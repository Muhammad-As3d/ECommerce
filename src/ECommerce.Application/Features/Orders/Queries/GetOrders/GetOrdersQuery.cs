using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Orders;
using ECommerce.Application.Specifications.OrderSpecifications;

namespace ECommerce.Application.Features.Orders.Queries.GetOrders;

public record GetOrdersQuery(FiltersRequest Spec) : IRequest<PaginatedList<OrderUserResponse>>;

public class GetOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetOrdersQuery, PaginatedList<OrderUserResponse>>
{
    public async Task<PaginatedList<OrderUserResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orderSpec = new OrderSpecification(currentUser.Id);

        return await unitOfWork
            .Repository<Order>()
            .GetAllPaginatedProjectAsync<OrderUserResponse>(orderSpec, request.Spec.PageNumber, request.Spec.PageSize, cancellationToken);
    }
}