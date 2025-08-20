namespace Bookistry.API.Contracts.Roles.Responses;

public record RoleResponse(
    string Id,
    string Name,
    bool IsDeleted
);
