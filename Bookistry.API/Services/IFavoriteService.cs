namespace Bookistry.API.Services;

public interface IFavoriteService
{
    Task<Result> CreateAsync(Guid bookId,string userId,CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid bookId, string userId, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsFavoriteAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<Result<UserFavoriteResponse>> GetUserFavoriteAsync(string userId, Guid bookId, CancellationToken cancellationToken = default);
}
