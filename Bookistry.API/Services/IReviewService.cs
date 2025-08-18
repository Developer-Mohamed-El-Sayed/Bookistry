namespace Bookistry.API.Services;

public interface IReviewService
{
    Task<Result<ReviewResponse>> CreateAsync(Guid bookId,string userId, ReviewRequest request, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<ReviewResponse>>> GetCurrentReview(Guid bookId, RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result<ReviewResponse>> GetAsync(Guid bookId, Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid bookId, Guid id, ReviewRequest request, CancellationToken cancellationToken = default);
}
