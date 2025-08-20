namespace Bookistry.API.Services;

public interface IAccountService
{
    Task<Result<UserProfileResponse>> GetUserProfile(string userId);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<Result> UpdateProfileAsync(string userId, UpdateUserProfileRequest request);
}
