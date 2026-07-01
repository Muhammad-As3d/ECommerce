namespace ECommerce.Application.Contracts.Authentication;

public record ConfirmationEmailRequest(
    string UserId,
    string Code
);
