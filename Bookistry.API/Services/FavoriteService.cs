namespace Bookistry.API.Services;

public class FavoriteService(ApplicationDbContext context, ILogger<FavoriteService> logger) : IFavoriteService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<FavoriteService> _logger = logger;

    public async Task<Result> CreateAsync(Guid bookId, string userId, CancellationToken cancellationToken = default)
    {
        var bookExists = await GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure(BookErrors.NotFound);
        var favoriteExists = await _context.Favorites
            .AnyAsync(f => f.BookId == bookId && f.UserId == userId, cancellationToken);
        if (favoriteExists)
            return Result.Failure(FavoriteErrors.AlreadyExists);
        var favorite = new Favorite
        {
            BookId = bookId,
            UserId = userId
        };
        await _context.AddAsync(favorite,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User {UserId} favorited book {BookId} successfully", userId, bookId);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid bookId, string userId, CancellationToken cancellationToken = default)
    {
        var bookExists = await GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure(BookErrors.NotFound);
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId, cancellationToken);
        if (favorite is null)
            return Result.Failure(FavoriteErrors.NotFound);
        await _context.Favorites
            .Where(f => f.BookId == bookId && f.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
        _logger.LogInformation("User {UserId} unfavorited book {BookId} successfully", userId, bookId);
        return Result.Success();
    }
    public async Task<Result<bool>> IsFavoriteAsync(Guid bookId, string userId, CancellationToken cancellationToken = default)
    {
        var bookExists = await GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure<bool>(BookErrors.NotFound);
        var isFavorite = await _context.Favorites
            .AnyAsync(x => x.BookId == bookId && x.UserId == userId, cancellationToken);

        return Result.Success(isFavorite);
    }
    private async Task<bool> GetBookExistsAsync(Guid bookId, CancellationToken cancellationToken) =>
        await _context.Books
            .AnyAsync(b => b.Id == bookId, cancellationToken);
}
