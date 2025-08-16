namespace Bookistry.API.Services;

public class ReviewService(
    ApplicationDbContext context,
    IBookHelpers bookHelpers,
    HybridCache hybridCache
    ) : IReviewService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IBookHelpers _bookHelpers = bookHelpers;
    private readonly HybridCache _hybridCache = hybridCache;
    private const string _cachePrefixKey = "reviews:";

    public async Task<Result<ReviewResponse>> CreateAsync(Guid bookId, string userId, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        var bookExists = await _bookHelpers.GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure<ReviewResponse>(BookErrors.NotFound);
        var reviewExists = await _context.Reviews
            .AnyAsync(r => r.BookId == bookId && r.ReviewerId == userId, cancellationToken);
        if (reviewExists)
            return Result.Failure<ReviewResponse>(ReviewErrors.AlreadyReviewed);
        var review = request.Adapt<Review>();
        review.BookId = bookId;
        review.ReviewerId = userId;
        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await _hybridCache.RemoveAsync($"{_cachePrefixKey}{bookId}", cancellationToken);
        return Result.Success(review.Adapt<ReviewResponse>());
    }

    public async Task<Result<PaginatedList<ReviewResponse>>> GetCurrentReview(Guid bookId, RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var bookExists = await _bookHelpers.GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure<PaginatedList<ReviewResponse>>(BookErrors.NotFound);
        var cacheKey = $"{_cachePrefixKey}{bookId}:{filters.PageNumber}:{filters.PageSize}";
        var paginatedReviews = await _hybridCache.GetOrCreateAsync(cacheKey, async _ =>
        {
            var query = _context.Reviews
                .AsNoTracking()
                .Where(r => r.BookId == bookId &&
                    (string.IsNullOrEmpty(filters.SearchTerm) ||
                     r.Comment.Contains(filters.SearchTerm) ||
                     r.Reviewer.UserName!.Contains(filters.SearchTerm)))
                .ProjectToType<ReviewResponse>();

            return await PaginatedList<ReviewResponse>.CreateAsync(
                query,
                filters.PageNumber,
                filters.PageSize,
                cancellationToken);
        }, cancellationToken: cancellationToken);

        return Result.Success(paginatedReviews);
    }

    // TODO: Add GetReviewByIdAsync(reviewId) 
    // - Check if review exists
    // - Return mapped response

    // TODO: Add UpdateReviewAsync(reviewId, userId, request) 
    // - Ensure review exists and belongs to user
    // - Update comment/rating
    // - Invalidate cache for book reviews

    // TODO: Add DeleteReviewAsync(reviewId, userId) 
    // - Ensure review exists and belongs to user
    // - Delete review
    // - Invalidate cache

    // TODO: Add GetBookReviewStatsAsync(bookId) 
    // - Return total reviews + average rating
    // - Cache this for performance
}
