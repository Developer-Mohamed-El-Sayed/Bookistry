namespace Bookistry.API.Errors;

public record SubscriptionErrors
{
    public static readonly Error UserHasActiveSubscription =
        new("Subscriptions.UserHasActiveSubscription", "The user already has an active subscription.", StatusCodes.Status409Conflict);
    public static readonly Error SubscriptionNotFound =
        new("Subscriptions.SubscriptionNotFound", "The requested subscription was not found.", StatusCodes.Status404NotFound);
    public static readonly Error SubscriptionAlreadyCancelled =
        new("Subscriptions.SubscriptionAlreadyCancelled", "The subscription has already been cancelled.", StatusCodes.Status400BadRequest);
}
