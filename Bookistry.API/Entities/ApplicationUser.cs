namespace Bookistry.API.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;    
    public string LastName { get; set; } = string.Empty;    
    public bool IsDisabled { get; set; }
    public bool IsVIP { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<ReadingProgress> ReadingProgresses { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<Favorite> Favorites { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
}
