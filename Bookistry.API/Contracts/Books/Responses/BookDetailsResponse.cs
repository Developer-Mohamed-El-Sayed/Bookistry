namespace Bookistry.API.Contracts.Books.Responses;

public record BookDetailsResponse(
    Guid Id,
    string Title,
    string Description,
    string CoverImageUrl,
    string PdfFileUrl,
    DateTime PublishedOn,
    string AccessLevel, // "Free" or "VIP"
    double AverageRating,
    int ViewCount,
    int DownloadCount,
    int PageCount,
    string AuthorFullName
// TODO: Add IEnumerable<ReviewResponse> Reviews when review feature is ready
// TODO: IEnumerable<gategoryresponse> Categories // Uncomment when categories are implemented
);
