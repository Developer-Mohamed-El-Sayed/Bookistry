namespace Bookistry.API.Errors;

public record UserErrors
{
    public static readonly Error DublicatedEmail =
        new("Auth.DuplicatedEmail", "This email is already registered", StatusCodes.Status409Conflict);
    public static readonly Error InvalidCredentials =
        new("Auth.InvalidCredentials", "Invalid email or password", StatusCodes.Status401Unauthorized);
    public static readonly Error UserDisabled =
        new("Auth.UserDisabled", "This account has been disabled", StatusCodes.Status403Forbidden);
    public static readonly Error UserLockedOut =
        new("Auth.UserLockedOut", "This account is locked due to multiple failed login attempts. Please try again later.", StatusCodes.Status423Locked);

}
