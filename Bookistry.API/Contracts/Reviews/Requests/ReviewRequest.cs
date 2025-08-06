namespace Bookistry.API.Contracts.Reviews.Requests;

public record ReviewRequest(
    string Comment,
    int Rating
);
