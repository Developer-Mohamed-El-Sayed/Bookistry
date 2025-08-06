namespace Bookistry.API.Contracts.Reviews.Responses;

public record ReviewResponse(
    string UserName,
    int Rating,
    string Comment,
    DateTime CreatedOn
);
