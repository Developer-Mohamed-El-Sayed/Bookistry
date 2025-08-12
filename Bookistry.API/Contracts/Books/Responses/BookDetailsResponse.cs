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
    string AuthorName,
    IEnumerable<CategoryResponse> Categories,
    IEnumerable<ReviewResponse> Reviews
);
