namespace Bookistry.API.Errors;

public record RoleErrors
{
    public static readonly Error RoleNotFound = 
        new("RoleNotFound","The specified role was not found.",StatusCodes.Status404NotFound );
    public static readonly Error RoleAlreadyExists =
        new("RoleAlreadyExists", "A role with the same name already exists.", StatusCodes.Status409Conflict);
}
