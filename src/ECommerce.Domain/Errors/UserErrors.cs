using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class UserErrors
{
    public static Error NotFound(string UserId) =>
        Error.NotFound("User.NotFound", $"User with ID '{UserId}' was not found");
    public static Error DuplicatedEmail =>
        Error.Conflict("User.DuplicatedEmail", "You entered email is already exists");
    public static Error EmailNotFound =>
        Error.NotFound("User.DuplicatedEmail", "You entered email is already exists");
    public static Error EmailIsConfirmed =>
        Error.BadRequest("User.EmailIsConfirmed", "Your email is already confirmed");
    public static Error EmailIsNotConfirmed =>
        Error.Conflict("User.EmailIsNotConfirmed", "Please confirm your email first.");
    public static Error InvalidCredentials =>
        Error.InvalidCredentials("User.InvalidCredentials", "Invalid Email/Password.");
    public static Error LockedUser =>
        Error.Unauthorized("User.LockedUser", "Please try again after 5 minutes");
    public static Error InvalidCode =>
        Error.Unauthorized("User.InvalidCode", "You entered Invalid Code");
    public static Error InvalidToken =>
        Error.Unauthorized("User.InvalidToken", "You entered Invalid token");
    public static Error InvalidRefreshToken =>
        Error.Unauthorized("User.InvalidRefreshToken", "You entered Invalid Refresh token");
    public static Error IsDisabled =>
    Error.InvalidCredentials("User.IsDisabled", "Disabled User, Please contact your administrator");
}