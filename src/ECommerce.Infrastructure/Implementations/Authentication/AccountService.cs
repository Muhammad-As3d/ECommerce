using ECommerce.Application.Contracts.Users;
using Mapster;

namespace ECommerce.Infrastructure.Implementations.Authentication;

public class AccountService(UserManager<User> userManager, ApplicationDbContext context) : IAccountService
{
    public async Task<UserProfileResponse> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
      => await userManager.Users
          .AsNoTracking()
          .Where(u => u.Id == userId)
          .ProjectToType<UserProfileResponse>()
          .SingleAsync(cancellationToken);

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        await context.Users
           .Where(x => x.Id == userId)
           .ExecuteUpdateAsync(s => s
               .SetProperty(x => x.FirstName, request.FirstName.Trim())
               .SetProperty(x => x.LastName, request.LastName.Trim())
               , cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);

        var result = await userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            return result.ToError();

        return Result.Success();
    }
}