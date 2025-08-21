namespace Bookistry.API.Contracts.SubscriptionPlans.Requests;

public record SubscriptionPlanRequest(
    string Name,
    decimal Price,
    int DurationInDays,
    string? Description,
    string Type
);
