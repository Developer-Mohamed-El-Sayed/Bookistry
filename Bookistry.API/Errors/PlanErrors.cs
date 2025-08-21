namespace Bookistry.API.Errors;

public record PlanErrors
{
    public static readonly Error PlanExists =
        new("Plans.PlanExists", "A subscription plan with this name already exists.", StatusCodes.Status409Conflict);
    public static readonly Error PlanNotFound =
        new("Plans.PlanNotFound", "The requested subscription plan was not found.", StatusCodes.Status404NotFound);
    public static readonly Error PlanAlreadyRevoked = 
        new("Plans.PlanAlreadyRevoked", "The subscription plan has already been revoked.", StatusCodes.Status409Conflict);
}
