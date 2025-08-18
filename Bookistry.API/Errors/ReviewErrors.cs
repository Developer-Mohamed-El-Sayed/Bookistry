namespace Bookistry.API.Errors;

public record ReviewErrors
{
    public static readonly Error AlreadyReviewed = 
        new("Review.AlreadyReviewed","You have already submitted a review for this book.",StatusCodes.Status409Conflict);
    public static readonly Error NotFound =
        new("Review.NotFound", "this review not found by given id.", StatusCodes.Status404NotFound);
    public static readonly Error AlreadyDeleted =
        new("Book.AlreadyDeleted", "This book has already been deleted.", StatusCodes.Status400BadRequest);
}
