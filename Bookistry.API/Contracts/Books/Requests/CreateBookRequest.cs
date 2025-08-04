namespace Bookistry.API.Contracts.Books.Requests;

public record CreateBookRequest(
    string Title,
    string Description,
    bool IsVIP,
    int PageCount,
    IEnumerable<string> CategoriesTitle,
    IFormFile CoverImage,
    IFormFile PdfFile
);
