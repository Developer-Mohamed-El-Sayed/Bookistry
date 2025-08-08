namespace Bookistry.API.Mappings;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SignUpRequest, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed, src => true)
            .Map(dest => dest.FirstName, src => GetFirstName(src.FullName))
            .Map(dest => dest.LastName, src => GetLastName(src.FullName));

        config.NewConfig<Book, BookResponse>()
            .Map(dest => dest.AuthorFullName, src => $"{src.Author.FirstName.Trim()} {src.Author.LastName.Trim()}");

        config.NewConfig<CategoryRequest, Category>()
            .Map(dest => dest.Name, src => src.Title);
        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.Title, src => src.Name);

        config.NewConfig<Review, ReviewResponse>()
            .Map(dest => dest.UserName, src => src.Reviewer.UserName);

        config.NewConfig<ReadingProgress, ReadingProgressResponse>()
            .Map(dest => dest.Title, src => src.Book.Title)
            .Map(dest => dest.TotalPages, src => src.Book.PageCount);
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
