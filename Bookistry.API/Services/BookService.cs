namespace Bookistry.API.Services;

public class BookService(ApplicationDbContext context) : IBookService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaginatedList<BookResponse>>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Book>()
            .Include(x => x.Author)
            .Where(x =>
            (string.IsNullOrEmpty(filters.SearchTerm) || x.Title.Contains(filters.SearchTerm)) &&
            (string.IsNullOrEmpty(filters.FilterBy) ||
            (filters.FilterBy == SubscriptionType.VIP && x.IsVIP) ||
            (filters.FilterBy == SubscriptionType.Free && !x.IsVIP)))
            .ProjectToType<BookResponse>()
            .AsNoTracking();

        var response = await PaginatedList<BookResponse>.CreateAsync(query, filters.PageNumber, filters.PageSize,cancellationToken);
        return Result.Success(response);
    }
}
