namespace Bookistry.API.Errors;

public record ReviewErrors
{
    public static readonly Error AlreadyReviewed = 
        new("Review.AlreadyReviewed","You have already submitted a review for this book.",StatusCodes.Status409Conflict);
}
