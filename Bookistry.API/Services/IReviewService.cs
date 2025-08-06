namespace Bookistry.API.Services;

public interface IReviewService
{
    Task<Result<ReviewResponse>> CreateAsync(Guid bookId,string userId, ReviewRequest request, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<ReviewResponse>>> GetCurrentReview(Guid bookId, RequestFilters filters, CancellationToken cancellationToken = default);
}
