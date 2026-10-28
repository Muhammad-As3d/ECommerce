namespace ECommerce.Application.Features.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    ChangePasswordRequest Request
) : IRequest<Result>;