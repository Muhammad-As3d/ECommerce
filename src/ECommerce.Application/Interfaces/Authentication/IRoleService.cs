using ECommerce.Application.Contracts.Roles;

namespace ECommerce.Application.Interfaces.Authentication;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);
    Task<Result<RoleDetailResponse>> GetAsync(string roleId, CancellationToken cancellationToken);
    Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string Id, RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default);
    Task<List<string>?> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
}