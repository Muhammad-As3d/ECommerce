using ECommerce.Application.Contracts.Payments;

namespace ECommerce.Application.Features.Payments.Commands.CreatePayment;

public record CreatePaymentCommand(Guid OrderId) :
    IRequest<Result<CreatePaymentResponse>>;