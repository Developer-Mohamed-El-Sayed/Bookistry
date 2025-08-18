namespace Bookistry.API.Contracts.Favorites.Responses;

public record UserFavoriteResponse(
    string Title,
    string UserName,
    string CoverImageUrl,
    DateTime CreatedOn
);
