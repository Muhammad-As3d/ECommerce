namespace ECommerce.Application.Interfaces.Authentication;

public interface IAccountService
{
    Task<UserProfileResponse> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken);
}