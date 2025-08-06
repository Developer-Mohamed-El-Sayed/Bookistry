namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder
            .Property(r => r.Comment)
            .HasMaxLength(1000);

        builder
            .Property(r => r.Rating)
            .IsRequired();

        builder
            .HasOne(r => r.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId);

        builder
            .HasOne(r => r.Reviewer)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.ReviewerId);
        
        builder.HasIndex(r => new { r.BookId, r.ReviewerId })
            .IsUnique();
    }
}
