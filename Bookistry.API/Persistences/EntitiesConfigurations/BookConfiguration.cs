namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(d => d.Description)
               .IsRequired()
               .HasMaxLength(2000);

        builder.Property(c => c.CoverImageUrl)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(p => p.PdfFileUrl)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(p => p.PublishedOn)
               .IsRequired();

        builder.Property(i => i.IsVIP)
               .IsRequired();

        builder.Property(a => a.AverageRating)
               .IsRequired()
               .HasPrecision(3, 2);

        builder.Property(b => b.PageCount)
               .IsRequired();

        builder.HasIndex(b => b.Title);
        builder.HasIndex(b => b.IsVIP);
        builder.HasIndex(b => b.PublishedOn);
        builder.HasIndex(b => b.AverageRating);

        builder.HasOne(b => b.Author)
        .WithMany(u => u.Books)
        .HasForeignKey(b => b.AuthorId);
    }
}
