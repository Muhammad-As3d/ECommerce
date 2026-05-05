using ECommerce.Domain.Abstractions;

namespace ECommerce.Domain.Errors;

public static class UserErrors
{
    public static Error NotFound(string UserId) =>
        Error.NotFound("User.NotFound", $"User with ID '{UserId}' was not found");
    public static Error DuplicatedEmail =>
        Error.Conflict("User.DuplicatedEmail", "You entered email is already exists");
    public static Error EmailIsConfirmed =>
        Error.Conflict("User.EmailIsConfirmed", "Your email is already confirmed");
    public static Error InvalidCode =>
        Error.Conflict("User.InvalidCode", "You entered Invalid Code");
}
