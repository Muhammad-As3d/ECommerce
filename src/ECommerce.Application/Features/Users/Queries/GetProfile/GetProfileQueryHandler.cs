namespace ECommerce.Application.Features.Users.Queries.GetProfile;

internal class GetProfileQueryHandler(IUserRepository userRepository, ICurrentUser currentUser) : IRequestHandler<GetProfileQuery, Result<UserProfileResponse>>
{
    public async Task<Result<UserProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await userRepository.GetProfileAsync(currentUser.Id, cancellationToken);

        return Result.Success(profile);
    }
}
