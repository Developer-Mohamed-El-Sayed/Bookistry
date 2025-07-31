namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class ReadingProgressConfiguration : IEntityTypeConfiguration<ReadingProgress>
{
    public void Configure(EntityTypeBuilder<ReadingProgress> builder)
    {
        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.UserId)
            .IsRequired();

        builder.Property(rp => rp.CurrentPage)
            .IsRequired();

        builder.Property(rp => rp.LastReadAt)
            .IsRequired();

        builder
            .HasIndex(rp => new { rp.BookId, rp.UserId })
            .IsUnique();

        builder
            .HasOne(rp => rp.Book)
            .WithMany(b => b.ReadingProgresses)
            .HasForeignKey(rp => rp.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(rp => rp.User)
            .WithMany(u => u.ReadingProgresses)
            .HasForeignKey(rp => rp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
