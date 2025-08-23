namespace Bookistry.API.Contracts.Subscriptions.Responses;

public record SubscriptionResponse(
    Guid Id,
    string UserId,
    Guid PlanId,
    string PlanName,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
);
