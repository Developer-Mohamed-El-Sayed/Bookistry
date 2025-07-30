namespace Bookistry.API.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;    
    public bool IsDisabled { get; set; }
    public bool IsVIP { get; set; }
}
