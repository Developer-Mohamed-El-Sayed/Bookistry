namespace Bookistry.API.Services;

public class ReadingProgressService(ApplicationDbContext context, IBookHelpers bookHelpers) : IReadingProgressService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IBookHelpers _bookHelpers = bookHelpers;

    public async Task<Result<ReadingProgressResponse>> GetAsync(string userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var readingProgress = await _context.ReadingProgresses
                .AsNoTracking()
                .Include(x => x.Book)
                .Where(x => x.BookId == bookId && x.UserId == userId)
                .ProjectToType<ReadingProgressResponse>()
                .FirstOrDefaultAsync(cancellationToken);

        if (readingProgress is null)
            return Result.Failure<ReadingProgressResponse>(ReadingProgressErrors.NotFound);

        return Result.Success(readingProgress);
    }

    public async Task<Result> UpdateAsync(string userId, Guid bookId, ReadingProgressRequest request, CancellationToken cancellationToken = default)
    {
        var bookExists = await _bookHelpers.GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure(BookErrors.NotFound);
        var readingProgress = await _context.ReadingProgresses
            .FirstOrDefaultAsync(rp => rp.BookId == bookId && rp.UserId == userId, cancellationToken);
        if (readingProgress is null)
        {
            readingProgress = request.Adapt<ReadingProgress>();
            
                readingProgress.BookId = bookId;
                readingProgress.UserId = userId;
            
            await _context.AddAsync(readingProgress,cancellationToken);
        }
        else
        {
            readingProgress.CurrentPage = request.CurrentPage;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
