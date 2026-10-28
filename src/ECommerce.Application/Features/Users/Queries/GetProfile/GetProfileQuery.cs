namespace ECommerce.Application.Features.Users.Queries.GetProfile;

public record GetProfileQuery() : IRequest<Result<UserProfileResponse>>;
