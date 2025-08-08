namespace Bookistry.API.Contracts.BookReadingProgress.Responses;

public record ReadingProgressResponse(
    string Title,
    int CurrentPage,
    int TotalPages,
    DateTime LastReadAt
);
