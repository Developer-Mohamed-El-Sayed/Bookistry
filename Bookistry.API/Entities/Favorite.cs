namespace Bookistry.API.Entities;

public sealed class Favorite  : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookId { get; set; }
    public string UserId { get; set; } = default!;

    public Book Book { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}
