namespace ECommerce.Application.Contracts.Users;

public record UserProfileResponse(
    string Id,
    string Email,
    string UserName,
    string FirstName,
    string LastName
);