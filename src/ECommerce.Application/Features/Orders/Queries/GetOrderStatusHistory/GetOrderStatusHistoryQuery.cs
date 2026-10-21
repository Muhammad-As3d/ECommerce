namespace ECommerce.Application.Features.Orders.Queries.GetOrderStatusHistory;

public sealed record GetOrderStatusHistoryQuery(
    Guid OrderId,
    bool CanAccessAnyOrder) : IRequest<Result<OrderStatusHistoryResponse>>;