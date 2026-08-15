namespace ECommerce.Application.Features.Orders.Commands.StartProcessing;

public class StartProcessingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<StartProcessingCommand, Result<StartProcessingResponse>>
{
    public async Task<Result<StartProcessingResponse>> Handle(StartProcessingCommand request, CancellationToken cancellationToken)
    {
        var orderRepository = unitOfWork.Repository<Order>();

        var orderInfo = await orderRepository
            .GetByPredicateProjectAsync(o => o.Id == request.OrderId,
                o => new StartProcessingResponse(
                    o.Id,
                    o.OrderNumber,
                    o.RowVersion,
                    o.Status,
                    o.UpdatedOn
                ), cancellationToken);

        if (orderInfo is null)
            return Result.Failure<StartProcessingResponse>(OrderErrors.NotFound(request.OrderId));

        if (orderInfo.Status != OrderStatus.Confirmed)
            return Result.Failure<StartProcessingResponse>(OrderErrors.StatusNotConfirmed);

        var order = Order.CreateStub(orderInfo.Id, orderInfo.RowVersion, orderInfo.Status);
        var updatedProperties = order.StartProcessing();
        orderRepository.PartialUpdate(order, updatedProperties);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new StartProcessingResponse(orderInfo.Id, orderInfo.OrderNumber,
            order.RowVersion, order.Status, order.UpdatedOn));
    }
}