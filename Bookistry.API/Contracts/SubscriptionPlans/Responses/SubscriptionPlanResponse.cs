namespace Bookistry.API.Contracts.SubscriptionPlans.Responses;

public record SubscriptionPlanResponse(
    Guid Id,
    string Name,
    decimal Price,
    int DurationInDays,
    string? Description,
    string Type
);
