namespace Bookistry.API.Mappings;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SignUpRequest, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed, src => true)
            .Map(dest => dest.FirstName, src => GetFirstName(src.FullName))
            .Map(dest => dest.LastName, src => GetLastName(src.FullName));

    }
    private static string GetFirstName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[0] : string.Empty;
    }

    private static string GetLastName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length <= 1) return string.Empty;

        return string.Join(' ', parts.Skip(1));
    }

}
