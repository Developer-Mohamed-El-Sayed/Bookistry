namespace Bookistry.API.Services;

public interface IBookService
{
    Task<Result<PaginatedList<BookResponse>>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken = default);
}
