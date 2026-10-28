using ECommerce.Application.Contracts.Users;

namespace ECommerce.Application.Features.Users.Commands.UpdateProfile;

public record UpdateProfileCommand(
    UpdateProfileRequest Request
) : IRequest<Result>;
