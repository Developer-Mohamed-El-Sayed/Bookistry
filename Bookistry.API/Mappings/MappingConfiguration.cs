namespace Bookistry.API.Mappings;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // User mappings
        config.NewConfig<SignUpRequest, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed, src => true)
            .Map(dest => dest.FirstName, src => GetFirstName(src.FullName))
            .Map(dest => dest.LastName, src => GetLastName(src.FullName));

        // Book mappings - Fixed duplicate mapping and added proper handling
        config.NewConfig<Book, BookResponse>()
            .Map(dest => dest.AuthorName, src => $"{src.Author.FirstName.Trim()} {src.Author.LastName.Trim()}")
            .Map(dest => dest.CoverImageUrl, src => GenerateCoverImageUrl(src.CoverImageUpload.StoredFileName))
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.PublishedOn, src => src.PublishedOn)
            .Map(dest => dest.IsVIP, src => src.IsVIP)
            .Map(dest => dest.AverageRating, src => src.AverageRating)
            .Map(dest => dest.PageCount, src => src.PageCount);

        // Category mappings
        config.NewConfig<CategoryRequest, Category>()
            .Map(dest => dest.Name, src => src.Title)
            .Map(dest => dest.Description, src => src.Description);

        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.Title, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        // Review mappings
        config.NewConfig<Review, ReviewResponse>()
            .Map(dest => dest.UserName, src => src.Reviewer.UserName);

        // Reading Progress mappings
        config.NewConfig<ReadingProgress, ReadingProgressResponse>()
            .Map(dest => dest.Title, src => src.Book.Title)
            .Map(dest => dest.TotalPages, src => src.Book.PageCount);

        // CreateBookRequest to Book mapping (if needed)
        config.NewConfig<CreateBookRequest, Book>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.IsVIP, src => src.IsVIP)
            .Map(dest => dest.PageCount, src => src.PageCount);

        config.NewConfig<ApplicationUser, UserProfileResponse>()
        .Map(dest => dest.FullName,
             src => (
                 (src.FirstName ?? "").Trim() + " " + (src.LastName ?? "").Trim()
             ).Trim()
        );
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

    private static string GenerateCoverImageUrl(string storedFileName)
    {
        if (string.IsNullOrWhiteSpace(storedFileName))
            return string.Empty;

        return $"/images/{storedFileName}";
    }
}