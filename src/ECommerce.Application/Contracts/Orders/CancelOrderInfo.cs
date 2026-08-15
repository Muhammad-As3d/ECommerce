using ECommerce.Domain.Enums;

namespace ECommerce.Application.Contracts.Orders;

public record CancelOrderInfo(
    Guid Id,
    OrderStatus Status,
    string OrderNumber,
    byte[] RowVersion,
    string? CancellationReason,
    List<CancelOrderProductInfo> ProductInfo);

public record CancelOrderProductInfo(
    Guid? ProductId,
    int Quantity);