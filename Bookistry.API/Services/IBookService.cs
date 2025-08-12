namespace Bookistry.API.Services;

public interface IBookService
{
    Task<Result<PaginatedList<BookResponse>>> GetAllAsync(RequestFilters filters,CancellationToken cancellationToken = default);
    Task<Result<BookResponse>> CreateAsync(string authorId, CreateBookRequest request, CancellationToken cancellationToken = default);
    Task<Result<BookDetailsResponse>> GetAsync(Guid bookId, CancellationToken cancellationToken = default);
}
