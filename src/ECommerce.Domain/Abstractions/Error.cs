namespace ECommerce.Domain.Abstractions;

public record Error(string Code, string Description, ErrorType ErrorType)
{
    public object[] Arguments { get; init; } = [];

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error Failure(
        string code, string description, params object[] arguments) =>
        new(code, description, ErrorType.Failure) { Arguments = arguments };

    public static Error NotFound(
        string code, string description, params object[] arguments) =>
        new(code, description, ErrorType.NotFound) { Arguments = arguments };

    public static Error Conflict(
        string code, string description, params object[] arguments) =>
        new(code, description, ErrorType.Conflict) { Arguments = arguments };

    public static Error BadRequest(
        string code, string description, params object[] arguments) =>
        new(code, description, ErrorType.BadRequest) { Arguments = arguments };

    public static Error Unauthorized(
        string code, string description, params object[] arguments) =>
        new(code, description, ErrorType.Unauthorized) { Arguments = arguments };

    public static Error Forbidden(
        string code, string description, params object[] arguments) =>
        new(code, description, ErrorType.Forbidden) { Arguments = arguments };

    public static Error InvalidCredentials(
        string code, string description, params object[] arguments) =>
        new(code, description, ErrorType.InvalidCredentials) { Arguments = arguments };
}

public enum ErrorType { Failure, NotFound, Conflict, BadRequest, Unauthorized, Forbidden, InvalidCredentials }
