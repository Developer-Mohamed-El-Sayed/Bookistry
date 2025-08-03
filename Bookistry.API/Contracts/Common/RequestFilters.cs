namespace Bookistry.API.Contracts.Common;

public record RequestFilters(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    string? FilterBy
);
