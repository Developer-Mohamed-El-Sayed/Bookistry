namespace Bookistry.API.Contracts.Books.Responses;

public record PdfDownloadResponse(
    byte[] FileContent,
    string ContentType,
    string FileName
);
