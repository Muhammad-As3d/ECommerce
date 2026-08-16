namespace ECommerce.Application.Features.Orders.Queries.GetOrderStatusHistory;

public sealed record GetOrderStatusHistoryQuery(Guid OrderId, bool CanAccessAnyOrder)
    : IRequest<Result<OrderStatusHistoryResponse>>;

internal sealed class GetOrderStatusHistoryQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetOrderStatusHistoryQuery, Result<OrderStatusHistoryResponse>>
{
    public async Task<Result<OrderStatusHistoryResponse>> Handle(
        GetOrderStatusHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var response = await unitOfWork.Repository<Order>().GetByPredicateProjectAsync(
            order => order.Id == request.OrderId && (request.CanAccessAnyOrder || order.UserId == currentUser.Id),
            o => new OrderStatusHistoryResponse(
                o.Id,
                o.StatusHistory
                    .OrderBy(history => history.ChangedAt)
                    .Select(h => new OrderStatusHistoryItemResponse(
                        h.ToStatus,
                        h.ChangedAt)).ToList()),
            cancellationToken);

        return response is null
            ? Result.Failure<OrderStatusHistoryResponse>(OrderErrors.NotFound(request.OrderId))
            : Result.Success(response);
    }
}
