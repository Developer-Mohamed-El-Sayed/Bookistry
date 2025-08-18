namespace Bookistry.API.Services;

public class FavoriteService(ApplicationDbContext context,
    ILogger<FavoriteService> logger,
    IBookHelpers bookHelpers,
    IWebHostEnvironment webHostEnvironment
    ) : IFavoriteService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<FavoriteService> _logger = logger;
    private readonly IBookHelpers _bookHelpers = bookHelpers;
    private readonly string _imagePath = $"{webHostEnvironment.WebRootPath}/images";

    public async Task<Result> CreateAsync(Guid bookId, string userId, CancellationToken cancellationToken = default)
    {
        var bookExists = await _bookHelpers.GetBookExistsAsync(bookId, cancellationToken);
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
        var bookExists = await _bookHelpers.GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure(BookErrors.NotFound);
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId, cancellationToken);
        if (favorite is null)
            return Result.Failure(FavoriteErrors.NotFound);
        favorite.IsDeleted = true;
        favorite.DeletedOn = DateTime.UtcNow;
        _context.Update(favorite);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User {UserId} unfavorited book {BookId} successfully", userId, bookId);
        return Result.Success();
    }
    public async Task<Result<bool>> IsFavoriteAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var bookExists = await _bookHelpers.GetBookExistsAsync(bookId, cancellationToken);
        if (!bookExists)
            return Result.Failure<bool>(BookErrors.NotFound);
        var isFavorite = await _context.Favorites
            .AnyAsync(x => x.BookId == bookId && !x.IsDeleted, cancellationToken);

        return Result.Success(isFavorite);
    }
    public async Task<Result<UserFavoriteResponse>> GetUserFavoriteAsync(string userId,Guid bookId,CancellationToken cancellationToken = default)
    {
        var favorites = await _context.Favorites
            .Where(f => f.UserId == userId && f.BookId == bookId && !f.IsDeleted)
            .AsNoTracking()
            .Select(f => new UserFavoriteResponse(
                f.Book.Title,
                f.User.UserName!,
                $"{_imagePath}/{f.Book.CoverImageUpload.StoredFileName}"
                , f.CreatedOn
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (!(favorites is not null))
            return Result.Failure<UserFavoriteResponse>(FavoriteErrors.NotFound);
        _logger.LogInformation("User {UserId} favorite books", userId);
        return Result.Success(favorites);
    }

}
