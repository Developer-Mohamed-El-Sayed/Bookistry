namespace Bookistry.API.Helpers;

public interface IBookHelpers
{
    Task<bool> GetBookExistsAsync(Guid bookId, CancellationToken cancellationToken = default);
}
