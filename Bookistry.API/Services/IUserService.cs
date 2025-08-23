namespace Bookistry.API.Services;

public interface IUserService
{
    Task<Result> UnlockAsync(string userId);
    Task<Result<IEnumerable<UserResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetAsync(string id);
    Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string id);


    //Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    //Task<Result> ToggleStatusAsync(string id);
    //Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
}
