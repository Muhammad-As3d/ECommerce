namespace ECommerce.Application.Contracts.Users;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);
