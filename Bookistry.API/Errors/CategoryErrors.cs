namespace Bookistry.API.Errors;

public record CategoryErrors
{
    public static readonly Error DuplicateTitle =
        new("Category.DuplicateTitle", "This category title already exists.",StatusCodes.Status409Conflict);
    public static readonly Error NotFound =
        new("Category.NotFound","The category was not found.",StatusCodes.Status404NotFound);
}
