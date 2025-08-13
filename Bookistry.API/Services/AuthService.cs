namespace Bookistry.API.Services;

public class AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider,
        SignInManager<ApplicationUser> signInManager
    )
    : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private const int _refreshTokenExpirationDays = 30;

    public async Task<Result<AuthResponse>> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);
        if(emailIsExists)
            return Result.Failure<AuthResponse>(UserErrors.DublicatedEmail);
        var user = request.Adapt<ApplicationUser>();

        user.UserName = GetUserName(request.Email);

        var result = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user,DefaultRoles.Reader.Name);
        if (result.Succeeded)
            return Result.Success(await GetAuthResponse(user));
        var error = result.Errors.First();
        return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result<AuthResponse>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
    {
        if(await _userManager.FindByEmailAsync(request.Email.ToLowerInvariant()) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.UserDisabled);
        var result = await _signInManager.PasswordSignInAsync(user,request.Password, false, true);
        // TODO: check if user is vip true
        if (result.Succeeded)
            return Result.Success(await GetAuthResponse(user));
        return result.IsLockedOut
           ?  Result.Failure<AuthResponse>(UserErrors.UserLockedOut)
           : Result.Failure<AuthResponse>(UserErrors.InvalidCredentials with { StatusCode = StatusCodes.Status400BadRequest });
    }
    public async Task<Result<AuthResponse>> SignInGoogleAsync(GoogleSignInRequest request)
    {
        if (await GoogleJsonWebSignature.ValidateAsync(request.IdToken) is not { } payload)
            return Result.Failure<AuthResponse>(UserErrors.InvalidGoogleToken);

        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = GetUserName(payload.Email),
                Email = payload.Email,
                FirstName = payload.GivenName ?? string.Empty,
                LastName = payload.FamilyName ?? string.Empty,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return Result.Failure<AuthResponse>(UserErrors.UserCreationFailed);

            await _userManager.AddToRoleAsync(user, DefaultRoles.Reader.Name);

            var loginInfo = new UserLoginInfo(Providers.Google, payload.Subject, Providers.Google);
            var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
            if (!addLoginResult.Succeeded)
                return Result.Failure<AuthResponse>(UserErrors.ExternalLoginFailed);
        }
        else
        {
            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.UserDisabled);

            if (await _userManager.IsLockedOutAsync(user))
                return Result.Failure<AuthResponse>(UserErrors.UserLockedOut);

            var logins = await _userManager.GetLoginsAsync(user);
            if (!logins.Any(x => x.LoginProvider == Providers.Google))
            {
                var loginInfo = new UserLoginInfo(Providers.Google, payload.Subject, Providers.Google);
                var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
                if (!addLoginResult.Succeeded)
                    return Result.Failure<AuthResponse>(UserErrors.ExternalLoginFailed);
            }
        }

        return Result.Success(await GetAuthResponse(user));
    }
    public async Task<Result> RevokeAsync(LogOutRequest request)
    {
        var userId = _jwtProvider.ValidateToken(request.Token);
        if (userId is null)
            return Result.Failure(UserErrors.InvalidToken);
        if(await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        var refreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == request.RefreshToken);
        if (refreshToken is null)
            return Result.Failure(UserErrors.InvalidToken);
        if (refreshToken.RevokedOn is not null)
            return Result.Failure(UserErrors.RefreshTokenAlreadyRevoked);

        if (refreshToken.ExpiresOn <= DateTime.UtcNow)
            return Result.Failure(UserErrors.RefreshTokenExpired);

        refreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }

    public async Task<Result<AuthResponse>> GenerateRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(request.Token);
        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);
        if(await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.NotFound);
        if(user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.UserDisabled);

        if (await _userManager.IsLockedOutAsync(user))
            return Result.Failure<AuthResponse>(UserErrors.UserLockedOut);

        var refreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == request.RefreshToken);
        if (refreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);
        if (refreshToken.RevokedOn is not null)
            return Result.Failure<AuthResponse>(UserErrors.RefreshTokenAlreadyRevoked);
        if (refreshToken.ExpiresOn <= DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.RefreshTokenExpired);
        refreshToken.RevokedOn = DateTime.UtcNow;
        var response = await GetAuthResponse(user);
        return Result.Success(response);
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
    private static string GenerateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private async Task<AuthResponse> GetAuthResponse(ApplicationUser user)
    {
        var userRoles = await GetRolesAsync(user);
        var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);
        return new AuthResponse(
            user.Id,
            user.Email!,
            GetFullName(user.FirstName, user.LastName),
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration
        );
    }
    private static string GetUserName(string? email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return string.Empty;
        return email.Split('@')[0];
    }

}
