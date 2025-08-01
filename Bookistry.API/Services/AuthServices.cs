namespace Bookistry.API.Services;

public class AuthServices(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider
    )
    : IAuthServices
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<AuthResponse>> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);
        if(emailIsExists)
            return Result.Failure<AuthResponse>(UserErrors.DublicatedEmail);
        var user = request.Adapt<ApplicationUser>();

        user.UserName = !string.IsNullOrEmpty(request.Email)
            && request.Email.Contains('@')
            ? request.Email.Split('@')[0]
            : string.Empty;

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user,DefaultRoles.Reader.Name);
            var userRoles = await GetRolesAsync(user);
            var (token,expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
            await _userManager.UpdateAsync(user);
            var response = new AuthResponse(
                user.Id,
                user.Email!,
                GetFullName(user.FirstName,user.LastName),
                token,
                expiresIn
            );
            return Result.Success(response);
            // TODO: send registration email Done
        }
        var error = result.Errors.First();
        return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    private async Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user) => 
         await _userManager.GetRolesAsync(user);
    private static string GetFullName(string firstName, string lastName)
    {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(firstName))
            builder.Append(firstName.Trim());

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            if (builder.Length > 0)
                builder.Append(' ');

            builder.Append(lastName.Trim());
        }
        return builder.ToString();
    }
}
