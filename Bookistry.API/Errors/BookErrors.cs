namespace Bookistry.API.Errors;

public record BookErrors
{
    public static readonly Error NotFound = 
        new("Book.NotFound","The book was not found.",StatusCodes.Status404NotFound);
}
