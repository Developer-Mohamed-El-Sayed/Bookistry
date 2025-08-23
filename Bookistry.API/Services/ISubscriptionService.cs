namespace Bookistry.API.Services;

public interface ISubscriptionService
{
    Task<Result<SubscriptionResponse>> CreateAsync(string userId,Guid planId,CancellationToken cancellationToken = default);
    Task<Result<SubscriptionResponse>> GetByIdAsync(Guid subscriptionId,CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<SubscriptionResponse>>> GetUserSubscriptionsAsync(string userId,CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SubscriptionResponse>>> GetUserSubscriptionsByPlanAsync(string userId, Guid planId,Guid subscriptionId, CancellationToken cancellationToken = default);
    Task<Result> RevokeAsync(string userId,Guid subscriptionId,Guid planId,CancellationToken cancellationToken = default);
    Task<Result> RenewAsync(string userId,Guid subscriptionId,Guid planId,CancellationToken cancellationToken = default);

}
