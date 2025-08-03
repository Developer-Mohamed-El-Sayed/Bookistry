namespace Bookistry.API.Contracts.Books.Responses;

public record BookResponse(
    Guid Id,
    string Title,
    string Description,
    string CoverImageUrl,
    string PdfFileUrl,
    DateTime PublishedOn,
    bool IsVIP,
    double AverageRating,
    int ViewCount,
    int DownloadCount,
    int PageCount,
    string AuthorFullName
// TODO: Add IEnumerable<ReviewResponse> Reviews when review feature is ready
// IEnumerable<gategoryresponse> Categories // Uncomment when categories are implemented
);
