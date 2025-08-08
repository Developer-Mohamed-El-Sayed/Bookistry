namespace Bookistry.API.Services;

public interface IReadingProgressService
{
    Task<Result> UpdateAsync(string userId,Guid bookId, ReadingProgressRequest request,CancellationToken cancellationToken = default);
    Task<Result<ReadingProgressResponse>> GetAsync(string userId, Guid bookId, CancellationToken cancellationToken = default);
}
