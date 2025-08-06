namespace Bookistry.API.Errors;

public record FavoriteErrors
{
    public static readonly Error AlreadyExists =
        new("Favorite.AlreadyExists", "The book is already added to your favorites.",StatusCodes.Status409Conflict);
    public static readonly Error NotFound =
        new("Favorite.NotFound", "Favorite not found for the selected book.", StatusCodes.Status404NotFound);

}
