namespace Bookistry.API.Services;

public class PlanService(ApplicationDbContext context) : IPlanService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<bool>> CheckPlanExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var isExists = await _context.SubscriptionPlans
            .AnyAsync(p => p.Id.Equals(id) && !p.IsDeleted, cancellationToken);
        return Result.Success(isExists);
    }

    public async Task<Result<SubscriptionPlanResponse>> CreateAsync(SubscriptionPlanRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.SubscriptionPlans.AnyAsync(p => p.Name.Equals(request.Name), cancellationToken))
            return Result.Failure<SubscriptionPlanResponse>(PlanErrors.PlanExists);
        var plan = request.Adapt<SubscriptionPlan>();
        plan.Type = request.Type.Equals(SubscriptionType.VIP, StringComparison.OrdinalIgnoreCase)
            ? SubscriptionType.VIP
            : SubscriptionType.Free;
        await _context.SubscriptionPlans.AddAsync(plan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(plan.Adapt<SubscriptionPlanResponse>());
    }

    public async Task<Result<SubscriptionPlanResponse>> GetPlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query =  await _context.SubscriptionPlans
            .Where(x => x.Id.Equals(id) && !x.IsDeleted)
            .AsNoTracking()
            .ProjectToType<SubscriptionPlanResponse>()
            .FirstOrDefaultAsync(cancellationToken);
        return query is not null 
            ? Result.Success(query) 
            : Result.Failure<SubscriptionPlanResponse>(PlanErrors.PlanNotFound);
    }

    public async Task<Result<IEnumerable<SubscriptionPlanResponse>>> GetPlansAsync(CancellationToken cancellationToken = default)
    {
        var query = await _context.SubscriptionPlans
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .ProjectToType<SubscriptionPlanResponse>()
            .ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<SubscriptionPlanResponse>>(query);
    }

    public async Task<Result<IEnumerable<SubscriptionPlanResponse>>> GetPlansByTypeAsync(string type,CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(type))
            return Result.Success<IEnumerable<SubscriptionPlanResponse>>([]);

        var normalizedType = type.Trim().ToLower();

        var query = await _context.SubscriptionPlans
            .Where(x => !x.IsDeleted && x.Type.ToLower() == normalizedType)
            .AsNoTracking()
            .ProjectToType<SubscriptionPlanResponse>()
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<SubscriptionPlanResponse>>(query);
    }

    public async Task<Result<IEnumerable<SubscriptionPlanResponse>>> SearchPlansAsync(string keyword, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrWhiteSpace(keyword))
            return Result.Success<IEnumerable<SubscriptionPlanResponse>>([]);

        var normalizedKeyword = keyword.Trim().ToLower();

        var query = await _context.SubscriptionPlans
            .Where(x => !x.IsDeleted &&
                (x.Name.ToLower().Contains(normalizedKeyword) ||
                 (x.Description != null && x.Description.ToLower().Contains(normalizedKeyword))))
            .AsNoTracking()
            .ProjectToType<SubscriptionPlanResponse>()
            .ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<SubscriptionPlanResponse>>(query);
    }

    public async Task<Result> UpdatePlanAsync(Guid id, SubscriptionPlanRequest request, CancellationToken cancellationToken = default)
    {
        var plan = await _context.SubscriptionPlans
            .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted, cancellationToken);
        if (plan is null)
            return Result.Failure(PlanErrors.PlanNotFound);
        if (await _context.SubscriptionPlans.AnyAsync(p => p.Name.Equals(request.Name) && !p.Id.Equals(id), cancellationToken))
            return Result.Failure(PlanErrors.PlanExists);
        plan = request.Adapt(plan);
        plan.Type = request.Type.Equals(SubscriptionType.VIP, StringComparison.OrdinalIgnoreCase)
            ? SubscriptionType.VIP
            : SubscriptionType.Free;

        _context.SubscriptionPlans.Update(plan);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RevokePlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var plan = await _context.SubscriptionPlans
        .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted, cancellationToken);
        if (plan is null)
            return Result.Failure(PlanErrors.PlanNotFound);
        if(plan.IsDeleted)
            return Result.Failure(PlanErrors.PlanAlreadyRevoked);
        await SetPlaneRevokedOrRenewedAsync(plan, true, cancellationToken);
        return Result.Success();
    }
    public async Task<Result> RenewPlanAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var plan = await _context.SubscriptionPlans
            .FirstOrDefaultAsync(x => x.Id.Equals(id) && x.IsDeleted, cancellationToken);
        if (plan is null)
            return Result.Failure(PlanErrors.PlanNotFound);
        await SetPlaneRevokedOrRenewedAsync(plan, false, cancellationToken);
        return Result.Success();
    }
    private  async Task SetPlaneRevokedOrRenewedAsync(SubscriptionPlan plan, bool isRevoked, CancellationToken cancellationToken)
    {
        plan.IsDeleted = isRevoked;
        plan.DeletedOn = isRevoked ? DateTime.UtcNow : null;
        _context.SubscriptionPlans.Update(plan);
        await _context.SaveChangesAsync(cancellationToken);
    }
   
}
