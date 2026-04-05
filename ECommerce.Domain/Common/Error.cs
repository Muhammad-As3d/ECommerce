namespace ECommerce.Domain.Common;

public record Error(string Code, string Description, ErrorType ErrorType)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error Failure(string Code, string Description) =>
        new(Code, Description, ErrorType.Failure);

    public static Error NotFound(string Code, string Description) =>
        new(Code, Description, ErrorType.NotFound);

    public static Error Conflict(string Code, string Description) =>
        new(Code, Description, ErrorType.Conflict);

    public static Error Validation(string Code, string Description) =>
        new(Code, Description, ErrorType.Validation);
}
public enum ErrorType { Failure, NotFound, Conflict, Validation }
