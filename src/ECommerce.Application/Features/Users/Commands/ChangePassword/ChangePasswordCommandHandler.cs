namespace ECommerce.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler(IUserRepository userRepository, ICurrentUser currentUser) : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) =>
        await userRepository.ChangePasswordAsync(currentUser.Id, request.Request, cancellationToken);
}
