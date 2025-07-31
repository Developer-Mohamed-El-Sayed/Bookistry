namespace Bookistry.API.Entities;

public sealed class Book : Auditable
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string PdfFileUrl { get; set; } = string.Empty;
    public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
    public bool IsVIP { get; set; }
    public double AverageRating { get; set; }
    public int ViewCount { get; set; }
    public int DownloadCount { get; set; }
    public int PageCount { get; set; }
    public string AuthorId { get; set; } = string.Empty;


    public ApplicationUser Author { get; set; } = default!;
    public ICollection<BookCategory> BookCategories { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<ReadingProgress> ReadingProgresses { get; set; } = [];
    public ICollection<Favorite> Favorites { get; set; } = [];

}
