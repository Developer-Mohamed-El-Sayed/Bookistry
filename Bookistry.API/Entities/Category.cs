namespace Bookistry.API.Entities;

public sealed class Category : Auditable
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<BookCategory> BookCategories { get; set; } = [];
}
