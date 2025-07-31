namespace Bookistry.API.Entities;

public sealed class BookCategory
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = default!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
