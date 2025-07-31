namespace Bookistry.API.Settings;

public abstract class Auditable
{

    public string CreatedById { get; set; } = default!;
    public string? UpdatedById { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
    [NotMapped]
    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser? UpdatedBy { get; set; }
}
