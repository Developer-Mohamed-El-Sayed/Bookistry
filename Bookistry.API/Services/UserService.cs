namespace Bookistry.API.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
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
