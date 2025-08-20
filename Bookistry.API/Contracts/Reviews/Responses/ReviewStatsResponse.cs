namespace Bookistry.API.Contracts.Reviews.Responses;

public record ReviewStatsResponse(
    int TotalReviews,
    double AverageRating,
    int FiveStarCount,
    int FourStarCount,
    int ThreeStarCount,
    int TwoStarCount,
    int OneStarCount
);
