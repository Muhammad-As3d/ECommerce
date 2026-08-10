using ECommerce.Application.Contracts.Orders;

namespace ECommerce.Application.Features.Orders.Queries.GetOrder;

public record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderDetailsResponse>>;

public class GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetOrderByIdQuery, Result<OrderDetailsResponse>>
{
    public async Task<Result<OrderDetailsResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork
            .Repository<Order>()
            .GetByPredicateProjectAsync<OrderDetailsResponse>(x => x.UserId == currentUser.Id && x.Id == request.Id, cancellationToken);

        if (order is null)
            return Result.Failure<OrderDetailsResponse>(OrderErrors.NotFound(request.Id));

        return Result.Success(order);
    }
}