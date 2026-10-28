namespace ECommerce.Application.Contracts.Users;

public record ResetPasswordRequest(
    string Email,
    string Code,
    string NewPassword
);