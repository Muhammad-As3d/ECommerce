using ECommerce.Application.Contracts.Payments;

namespace ECommerce.Application.Features.Payments.Commands.PaymobWebhook;

public record PaymobWebhookCommand(PaymobTransaction Transaction, string Hmac) : IRequest<Result>;
