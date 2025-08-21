namespace Bookistry.API.Entities;

public sealed class SubscriptionPlan : Auditable
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Name { get; set; } = default!;       
    public decimal Price { get; set; }                 
    public int DurationInDays { get; set; }             
    public string? Description { get; set; }

    public string Type { get; set; } = SubscriptionType.Free;

    public ICollection<Subscription> Subscriptions { get; set; } = [];
}
