namespace ECommerce.Domain.Abstractions;

public record Error(string Code, string Description, ErrorType ErrorType)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error Failure(string Code, string Description) =>
        new(Code, Description, ErrorType.Failure);
    public static Error NotFound(string Code, string Description) =>
        new(Code, Description, ErrorType.NotFound);
    public static Error Conflict(string Code, string Description) =>
        new(Code, Description, ErrorType.Conflict);
    public static Error BadRequest(string Code, string Description) =>
        new(Code, Description, ErrorType.BadRequest);
    public static Error Unauthorized(string Code, string Description) =>
        new(Code, Description, ErrorType.Unauthorized);
    public static Error Forbidden(string Code, string Description) =>
        new(Code, Description, ErrorType.Forbidden);
    public static Error InvalidCredentials(string Code, string Description) =>
        new(Code, Description, ErrorType.InvalidCredentials);
}
public enum ErrorType { Failure, NotFound, Conflict, BadRequest, Unauthorized, Forbidden, InvalidCredentials }
