namespace Bookistry.API.Helpers;

public class BookHelpers(ApplicationDbContext context) : IBookHelpers
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> GetBookExistsAsync(Guid bookId, CancellationToken cancellationToken = default) => 
        await _context.Books
            .AnyAsync(b => b.Id == bookId && !b.IsDeleted, cancellationToken);

}
