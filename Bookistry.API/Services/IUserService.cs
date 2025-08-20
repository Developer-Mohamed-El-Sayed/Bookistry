namespace Bookistry.API.Services;

public interface IUserService
{
    Task<Result> UnlockAsync(string userId);
}
