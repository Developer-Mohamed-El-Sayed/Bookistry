namespace Bookistry.API.Errors;

public record ReadingProgressErrors
{
    public static readonly Error NotFound = 
        new("ReadingProgress.NotFound","Reading progress not found for the specified book and user.",StatusCodes.Status404NotFound);
}
