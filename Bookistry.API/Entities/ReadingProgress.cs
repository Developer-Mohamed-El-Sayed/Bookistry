namespace Bookistry.API.Entities;

public sealed class ReadingProgress : Auditable
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid BookId { get; set; }
    public string UserId { get; set; } = default!;
    public int CurrentPage { get; set; }
    public DateTime LastReadAt { get; set; } = DateTime.UtcNow;
    public Book Book { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}
