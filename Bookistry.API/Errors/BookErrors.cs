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
    public static readonly Error VipRequired =
        new("Book.VipRequired", "VIP subscription is required to download this book.", StatusCodes.Status403Forbidden);
    public static readonly Error AlreadyDeleted =
        new("Book.AlreadyDeleted", "This book has already been deleted.", StatusCodes.Status400BadRequest);
}
