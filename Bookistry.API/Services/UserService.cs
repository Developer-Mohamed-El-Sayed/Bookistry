namespace Bookistry.API.Services;

public class UserService(UserManager<ApplicationUser> userManager,
    ApplicationDbContext context,
    IRoleService roleService,
    IUserHelpers userHelpers
    ) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly IRoleService _roleService = roleService;
    private readonly IUserHelpers _userHelpers = userHelpers;

    public async Task<Result<IEnumerable<UserResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = await _context.Users
            .Join(_context.UserRoles,
                  user => user.Id,
                  userRole => userRole.UserId,
                  (user, userRole) => new { User = user, UserRole = userRole })
            .Join(_context.Roles,
                  combined => combined.UserRole.RoleId,
                  role => role.Id,
                  (combined, role) => new { combined.User, Role = role })
            .Where(x => x.Role.Name != DefaultRoles.Reader.Name)
            .GroupBy(x => new {
                x.User.Id,
                x.User.FirstName,
                x.User.LastName,
                x.User.Email,
                x.User.IsDisabled
            })
            .Select(g => new UserResponse(
                g.Key.Id,
                g.Key.FirstName,
                g.Key.LastName,
                g.Key.Email!,
                g.Key.IsDisabled,
                g.Select(x => x.Role.Name!).Distinct().ToList()
            ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<UserResponse>>(query);
    }

    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        var query = await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserResponse(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email!,
                u.IsDisabled,
                _context.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles,
                          ur => ur.RoleId,
                          r => r.Id,
                          (ur, r) => r.Name!)
                    .ToList()
            ))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return query is null
            ? Result.Failure<UserResponse>(UserErrors.NotFound)
            : Result.Success(query);
    }


    public async Task<Result> UnlockAsync(string userId)
    {
        if(await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        var result = await _userManager.SetLockoutEndDateAsync(user, null);
        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code,error.Description,StatusCodes.Status423Locked));
    }
    public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if(await _userManager.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DublicatedEmail);
        var allowedRoles = await _roleService.GetAllRolesAsync(cancellationToken);
        if(request.Roles.Except(allowedRoles.Select(r => r.Name)).Any())
            return Result.Failure<UserResponse>(RoleErrors.InvalidRoles);
        var user = request.Adapt<ApplicationUser>();
        user.UserName = _userHelpers.GetUserName(request.Email);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, request.Roles);
            var response = (user,request.Roles).Adapt<UserResponse>();
            return Result.Success(response);
        }
        var error = result.Errors.First();
        return Result.Failure<UserResponse>(new Error(error.Code,error.Description,StatusCodes.Status422UnprocessableEntity));
    }
    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.NotFound);
        if (await _userManager.Users.AnyAsync(u => u.Email == request.Email && u.Id != id, cancellationToken))
            return Result.Failure(UserErrors.DublicatedEmail);
        var allowedRoles = await _roleService.GetAllRolesAsync(cancellationToken);
        if (request.Roles.Except(allowedRoles.Select(r => r.Name)).Any())
            return Result.Failure(RoleErrors.InvalidRoles);
        user = request.Adapt(user);
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            await _context.UserRoles
                .Where(x => x.UserId.Equals(id))
                .ExecuteDeleteAsync(cancellationToken);
            await _userManager.AddToRolesAsync(user, request.Roles);
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code,error.Description,StatusCodes.Status422UnprocessableEntity));
    }
    public async Task<Result> ToggleStatusAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.NotFound);
        user.IsDisabled = !user.IsDisabled;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return Result.Success();
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

}
