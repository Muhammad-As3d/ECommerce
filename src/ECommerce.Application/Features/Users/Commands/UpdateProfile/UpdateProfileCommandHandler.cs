namespace ECommerce.Application.Features.Users.Commands.UpdateProfile;

internal class UpdateProfileCommandHandler(ICurrentUser currentUser, IUserRepository userRepository)
    : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken) =>
        await userRepository.UpdateProfileAsync(currentUser.Id, request.Request, cancellationToken);
}
