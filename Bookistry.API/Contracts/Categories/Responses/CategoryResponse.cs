namespace Bookistry.API.Contracts.Categories.Responses;

public record CategoryResponse(
    Guid Id,
    string Title,
    string  Description
);
