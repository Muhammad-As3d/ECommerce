namespace ECommerce.Application.Interfaces.Authentication;

public interface IUserService
{
    Task<PaginatedList<UserResponse>> GetAllAsync(UserFiltersRequest filter, CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetAsync(string id);
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default);
    Task<Result> UnlockAsync(string id);
}