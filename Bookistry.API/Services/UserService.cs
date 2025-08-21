namespace Bookistry.API.Services;

public class UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

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


}
