namespace Bookistry.API.Services;

public class ReviewService(ApplicationDbContext context, IBookHelpers bookHelpers) : IReviewService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IBookHelpers _bookHelpers = bookHelpers;

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
        return Result.Success(review.Adapt<ReviewResponse>());
    }

    public async Task<Result<PaginatedList<ReviewResponse>>> GetCurrentReview(Guid bookId, RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var bookExists = await _bookHelpers.GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure<PaginatedList<ReviewResponse>>(BookErrors.NotFound);
        var reviewsQuery = _context.Reviews
            .AsNoTracking()
            .Where(r => r.BookId == bookId &&
                (string.IsNullOrEmpty(filters.SearchTerm) ||
                 r.Comment.Contains(filters.SearchTerm) ||
                 r.Reviewer.UserName!.Contains(filters.SearchTerm))) 
            .OrderByDescending(r => r.CreatedOn)
            .ProjectToType<ReviewResponse>();
        var paginatedReviews = await PaginatedList<ReviewResponse>.CreateAsync(reviewsQuery, filters.PageNumber, filters.PageSize, cancellationToken);
        return Result.Success(paginatedReviews);
    }
    
  
}
