namespace Bookistry.API.Entities;

public sealed class Review : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public double Rating { get; set; } // rating for every user
    public string Comment { get; set; } = string.Empty;

    public Guid BookId { get; set; }
    public string ReviewerId { get; set; } = string.Empty;


    public Book Book { get; set; } = default!;
    public ApplicationUser Reviewer { get; set; } = default!;
}
