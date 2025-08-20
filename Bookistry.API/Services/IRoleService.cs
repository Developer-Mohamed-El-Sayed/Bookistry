namespace Bookistry.API.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<Result<RoleResponse>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);
    Task<Result<RoleResponse>> CreateRoleAsync(RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result<RoleResponse>> UpdateRoleAsync(string roleId, RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleRoleAsync(string roleId, CancellationToken cancellationToken = default);
}
