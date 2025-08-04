namespace Bookistry.API.Contracts.Books.Responses;

public record BookResponse(
    Guid Id,
    string Title,
    string Description,
    string CoverImageUrl,
    DateTime PublishedOn,
    bool IsVIP,
    double AverageRating,
    int PageCount,
    string AuthorFullName
);
