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


        builder
            .OwnsOne(p => p.PdfFileUpload, pdf =>
            {
                pdf.ToTable("BookPdfFiles");
                pdf.WithOwner().HasForeignKey("BookId");
                pdf.Property(x => x.FileName).HasMaxLength(250);
                pdf.Property(x => x.StoredFileName).HasMaxLength(250);
                pdf.Property(x => x.FileExtension).HasMaxLength(10);
                pdf.Property(x => x.ContentType).HasMaxLength(50);                
            });

        builder
            .OwnsOne(i => i.CoverImageUpload, img =>
            {
                img.ToTable("BookCoverImages");
                img.WithOwner().HasForeignKey("BookId");
                img.Property(x => x.FileName).HasMaxLength(250);
                img.Property(x => x.StoredFileName).HasMaxLength(250);
                img.Property(x => x.FileExtension).HasMaxLength(10);
                img.Property(x => x.ContentType).HasMaxLength(50);
            });
    }
}
