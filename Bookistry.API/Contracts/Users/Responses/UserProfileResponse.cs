namespace Bookistry.API.Contracts.Users.Responses;

public record UserProfileResponse(
    string Id,
    string FullName,
    string Email,
    string ProfileAvatar
);
