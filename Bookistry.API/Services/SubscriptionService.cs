namespace Bookistry.API.Services;

public class SubscriptionService(ApplicationDbContext context) : ISubscriptionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<SubscriptionResponse>> CreateAsync(string userId, Guid planId, CancellationToken cancellationToken = default)
    {
        var plan = await _context.SubscriptionPlans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Equals(planId), cancellationToken);
        if (plan is null)
            return Result.Failure<SubscriptionResponse>(PlanErrors.PlanNotFound);

        var isActiveSubscription = await _context.Subscriptions
            .AsNoTracking()
            .AnyAsync(s => s.UserId.Equals(userId) && !s.IsDeleted, cancellationToken);
        if (isActiveSubscription)
            return Result.Failure<SubscriptionResponse>(SubscriptionErrors.UserHasActiveSubscription);
        var subscription = new Subscription
        {
            UserId = userId,
            PlanId = plan.Id,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(plan.DurationInDays)
        };
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync(cancellationToken);
        var response = new SubscriptionResponse(
            subscription.Id,
            userId,
            plan.Id,
            plan.Name,
            subscription.StartDate,
            subscription.EndDate,
            subscription.IsActive
        );
        return Result.Success(response);
    }

    public async Task<Result<SubscriptionResponse>> GetByIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        var subscription = await _context.Subscriptions
            .Where(x => x.Id.Equals(subscriptionId))
            .Include(s => s.Plan)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (subscription is null)
            return Result.Failure<SubscriptionResponse>(SubscriptionErrors.SubscriptionNotFound);
        var response = new SubscriptionResponse(
                subscription.Id,
                subscription.UserId,
                subscription.PlanId,
                subscription.Plan.Name,
                subscription.StartDate,
                subscription.EndDate,
                subscription.IsActive
           );
        return Result.Success(response);
    }

    public Task<Result<IEnumerable<SubscriptionResponse>>> GetUserSubscriptionsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var subscriptions = _context.Subscriptions
            .Where(s => s.UserId.Equals(userId))
            .Include(s => s.Plan)
            .AsNoTracking()
            .Select(subscription => new SubscriptionResponse(
                subscription.Id,
                subscription.UserId,
                subscription.PlanId,
                subscription.Plan.Name,
                subscription.StartDate,
                subscription.EndDate,
                subscription.IsActive
            ));
        return Task.FromResult(Result.Success<IEnumerable<SubscriptionResponse>>(subscriptions));
    }

    public async Task<Result<IEnumerable<SubscriptionResponse>>> GetUserSubscriptionsByPlanAsync(string userId, Guid planId,Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _context.Subscriptions
            .Where(s => s.UserId.Equals(userId) && s.PlanId.Equals(planId) && s.Id.Equals(subscriptionId))
            .Include(s => s.Plan)
            .AsNoTracking()
            .Select(subscription => new SubscriptionResponse(
                subscription.Id,
                subscription.UserId,
                subscription.PlanId,
                subscription.Plan.Name,
                subscription.StartDate,
                subscription.EndDate,
                subscription.IsActive
            )).ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<SubscriptionResponse>>(subscriptions);
    }

    public async Task<Result> RenewAsync(string userId, Guid subscriptionId, Guid planId, CancellationToken cancellationToken = default)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId && s.UserId == userId && s.PlanId == planId, cancellationToken);

        if (subscription is null)
            return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

        subscription.IsDeleted = false;
        subscription.DeletedOn = null;

        subscription.StartDate = DateTime.UtcNow;
        subscription.EndDate = DateTime.UtcNow.AddDays(subscription.Plan.DurationInDays); // assume your plan has duration

        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RevokeAsync(string userId, Guid subscriptionId, Guid planId, CancellationToken cancellationToken = default)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == subscriptionId && s.UserId == userId && s.PlanId == planId, cancellationToken);

        if (subscription is null)
            return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

        if (subscription.IsDeleted || !subscription.IsActive)
            return Result.Failure(SubscriptionErrors.SubscriptionAlreadyCancelled);

        subscription.IsDeleted = true;
        subscription.DeletedOn = DateTime.UtcNow;
        subscription.EndDate = DateTime.UtcNow; 

        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


}
