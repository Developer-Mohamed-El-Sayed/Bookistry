namespace Bookistry.API.Services;

public interface IPlanService
{
    Task<Result<SubscriptionPlanResponse>> CreateAsync(SubscriptionPlanRequest request,CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SubscriptionPlanResponse>>> GetPlansAsync(CancellationToken cancellationToken = default);
    Task<Result<SubscriptionPlanResponse>> GetPlanAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdatePlanAsync(Guid id, SubscriptionPlanRequest request, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SubscriptionPlanResponse>>> SearchPlansAsync(string keyword, CancellationToken cancellationToken = default);
    Task<Result<bool>> CheckPlanExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SubscriptionPlanResponse>>> GetPlansByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<Result> RevokePlanAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> RenewPlanAsync(Guid id, CancellationToken cancellationToken = default);

}
