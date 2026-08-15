namespace ECommerce.Application.Contracts.Orders;

public record StartProcessingResponse(
    Guid Id,
    string OrderNumber,
    byte[] RowVersion,
    OrderStatus Status,
    DateTime? UpdatedAt);
