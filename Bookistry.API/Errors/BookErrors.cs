namespace Bookistry.API.Errors;

public record BookErrors
{
    public static readonly Error NotFound = 
        new("Book.NotFound","The book was not found.",StatusCodes.Status404NotFound);
    public static readonly Error FileSaveError = 
        new("Book.FileSaveError","An error occurred while saving the book files",StatusCodes.Status400BadRequest);
    public static readonly Error DatabaseSaveError = 
        new("Book.DatabaseSaveError","An error occurred while saving the book to the database",StatusCodes.Status400BadRequest);
    public static readonly Error DublicatedTitle =
        new("Book.DublicatedTitle", "This title is already exists.", StatusCodes.Status409Conflict);
}
