namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.Property(p => p.PaymentToken)
            .IsRequired();

        builder.Property(p => p.PaymentGateway)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.IsPaid)
            .IsRequired();

        builder.Property(p => p.PaidAt);

        builder.Property(p => p.PaymentReferenceNumber)
            .HasMaxLength(100);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId);

        builder.HasOne(p => p.Subscription)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.SubscriptionId);
    }
}
