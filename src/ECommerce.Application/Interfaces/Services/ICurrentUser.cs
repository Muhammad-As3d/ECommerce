namespace ECommerce.Application.Interfaces.Services;

public interface ICurrentUser
{
    string Id { get; }
    string FullName { get; }
    string Email { get; }
}
