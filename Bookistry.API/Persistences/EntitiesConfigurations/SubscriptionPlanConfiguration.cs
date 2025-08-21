
namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .IsRequired();

        builder.Property(p => p.DurationInDays)
            .IsRequired();

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasMaxLength(500);


        builder
            .HasMany(p => p.Subscriptions)
            .WithOne(s => s.Plan)
            .HasForeignKey(s => s.PlanId);

        builder
            .HasIndex(p => p.Name)
            .IsUnique();
    }
}
