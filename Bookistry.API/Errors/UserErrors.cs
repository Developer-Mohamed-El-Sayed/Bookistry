namespace Bookistry.API.Errors;

public record UserErrors
{
    public static readonly Error DublicatedEmail =
        new("Auth.DuplicatedEmail", "This email is already registered", StatusCodes.Status409Conflict);

}
