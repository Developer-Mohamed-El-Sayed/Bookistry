
namespace Bookistry.API.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<Result<RoleResponse>> CreateRoleAsync(RoleRequest request, CancellationToken cancellationToken = default)
    {
        if(await _roleManager.RoleExistsAsync(request.Name))
            return Result.Failure<RoleResponse>(RoleErrors.RoleAlreadyExists);
        var role = new ApplicationRole
        {
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Name = request.Name
        };
        var result = await _roleManager.CreateAsync(role);
        if(result.Succeeded)
            return Result.Success(role.Adapt<RoleResponse>());
        var error = result.Errors.First();
        return Result.Failure<RoleResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var query = await _roleManager.Roles
            .Where(r => !r.IsDefault)
            .AsNoTracking()
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);
        return query;
    }

    public async Task<Result<RoleResponse>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
    {
        if(await _roleManager.FindByIdAsync(roleId) is not { } role)
            return Result.Failure<RoleResponse>(RoleErrors.RoleNotFound);
        var response = role.Adapt<RoleResponse>();
        return Result.Success(response);
    }

    public async Task<Result> ToggleRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Result.Failure<RoleResponse>(RoleErrors.RoleNotFound);
        role.IsDeleted = !role.IsDeleted;
        await _roleManager.UpdateAsync(role);
        return Result.Success();
    }

    public async Task<Result<RoleResponse>> UpdateRoleAsync(string roleId, RoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Result.Failure<RoleResponse>(RoleErrors.RoleNotFound);

        var nameExists = await _roleManager.Roles
            .AnyAsync(r => r.Name == request.Name && r.Id != roleId, cancellationToken);
        if (nameExists)
            return Result.Failure<RoleResponse>(RoleErrors.RoleAlreadyExists);

        role.Name = request.Name;
        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
            return Result.Success(role.Adapt<RoleResponse>());

        var error = result.Errors.First();
        return Result.Failure<RoleResponse>(
            new Error(error.Code, error.Description, StatusCodes.Status400BadRequest)
        );
    }

}
