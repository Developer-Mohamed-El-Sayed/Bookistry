namespace Bookistry.API.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> SignInGoogleAsync(GoogleSignInRequest request);
    Task<Result> RevokeAsync(LogOutRequest request);
    Task<Result<AuthResponse>> GenerateRefreshTokenAsync(RefreshTokenRequest request,CancellationToken cancellationToken = default);
}
