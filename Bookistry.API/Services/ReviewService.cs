namespace Bookistry.API.Services;

public class ReviewService(
    ApplicationDbContext context,
    IBookHelpers bookHelpers
    ) : IReviewService
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

        var query = _context.Reviews
            .AsNoTracking()
            .Where(r => r.BookId == bookId &&
                (string.IsNullOrEmpty(filters.SearchTerm) ||
                 r.Comment.Contains(filters.SearchTerm) ||
                 r.Reviewer.UserName!.Contains(filters.SearchTerm)))
            .ProjectToType<ReviewResponse>();

        var response = await PaginatedList<ReviewResponse>.CreateAsync(
            query,
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
    public async Task<Result<ReviewResponse>> GetAsync(Guid bookId, Guid id,CancellationToken cancellationToken = default)
    {
        var query = await _context.Reviews
            .AsNoTracking()
            .Include(x => x.Reviewer)
            .SingleOrDefaultAsync(x => x.Id.Equals(id) && x.BookId.Equals(bookId) && !x.IsDeleted, cancellationToken);
        if (query is null)
            return Result.Failure<ReviewResponse>(ReviewErrors.NotFound);

        var response = query.Adapt<ReviewResponse>();
        return Result.Success(response);
    }
    public async Task<Result> UpdateAsync(Guid bookId,Guid id, ReviewRequest request,CancellationToken cancellationToken = default)
    {
        var query = await _context.Reviews
            .SingleOrDefaultAsync(x => x.Id.Equals(id) && x.BookId.Equals(bookId) && !x.IsDeleted, cancellationToken);
        if(query is null)
            return Result.Failure(ReviewErrors.NotFound);
        await _context.Reviews
            .Where(x => x.Id.Equals(id) && x.BookId.Equals(bookId))
            .ExecuteUpdateAsync(setter =>
                setter
                .SetProperty(x => x.Comment, request.Comment)
                .SetProperty(x => x.Rating, request.Rating), cancellationToken: cancellationToken
            );
        return Result.Success();
    }
    public async Task<Result> DeleteAsync(Guid bookId,Guid id,CancellationToken cancellationToken = default)
    {
        var review = await _context.Reviews
          .SingleOrDefaultAsync(x => x.Id.Equals(id) && x.BookId.Equals(bookId), cancellationToken);
        if (review is null)
            return Result.Failure(ReviewErrors.NotFound);
        if (review.IsDeleted)
            return Result.Failure(ReviewErrors.AlreadyDeleted);
        await SetReviewDeleteAsync(review,true,cancellationToken);
        return Result.Success();
    }
    public async Task<Result> RestoreAsync(Guid bookId, Guid id, CancellationToken cancellationToken = default)
    {
        var review = await _context.Reviews
          .SingleOrDefaultAsync(x => x.Id.Equals(id) && x.BookId.Equals(bookId), cancellationToken);
        if (review is null)
            return Result.Failure(ReviewErrors.NotFound);
        await SetReviewDeleteAsync(review, false, cancellationToken);
        return Result.Success();
    }
    private async Task SetReviewDeleteAsync(Review review,bool isDelete,CancellationToken cancellationToken = default)
    {
        review.IsDeleted = isDelete;
        review.DeletedOn = isDelete ? DateTime.UtcNow : null;
        _context.Update(review);
        await _context.SaveChangesAsync(cancellationToken);
    }
    // TODO: GetBookReviewStatsAsync(bookId) - statistics and average rating
}
