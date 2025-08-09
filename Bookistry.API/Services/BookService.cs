namespace Bookistry.API.Services;

public class BookService(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment webHostEnvironment,
        ILogger<BookService> logger
    ) : IBookService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<BookService> _logger = logger;
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/files";
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";

    public async Task<Result<BookResponse>> CreateAsync(string authorId, CreateBookRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting CreateAsync for authorId: {AuthorId}", authorId);

        if (await _userManager.FindByIdAsync(authorId) is not { } author)
            return Result.Failure<BookResponse>(UserErrors.NotFound);

        if (await _userManager.IsInRoleAsync(author, DefaultRoles.Author.Name) is false)
            return Result.Failure<BookResponse>(UserErrors.NotAuthor);

        var randomFileName = Path.GetRandomFileName();
        var randomImageName = Path.GetRandomFileName();
        _logger.LogInformation("Generated file names: Pdf={PdfFile}, Image={ImageFile}", randomFileName, randomImageName);

        var book = new Book
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            Description = request.Description,
            IsVIP = request.IsVIP,
            PageCount = request.PageCount,
            AuthorId = authorId, // Critical: Set the AuthorId
            CoverImageUpload = new UploadedFile
            {
                FileName = request.CoverImage.FileName,
                StoredFileName = randomImageName,
                FileExtension = Path.GetExtension(request.CoverImage.FileName),
                ContentType = request.CoverImage.ContentType
            },
            PdfFileUpload = new UploadedFile
            {
                FileName = request.PdfFile.FileName,
                StoredFileName = randomFileName,
                FileExtension = Path.GetExtension(request.PdfFile.FileName),
                ContentType = request.PdfFile.ContentType
            },
            BookCategories = []
        };

       
        _logger.LogInformation("Processing {Count} categories", request.CategoryDetails.Count());
        foreach (var categoryRequest in request.CategoryDetails)
        {
            _logger.LogInformation("Processing category: {CategoryTitle}", categoryRequest.Title);

            var categoryEntity = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryRequest.Title, cancellationToken);

            if (categoryEntity is null)
            {
                _logger.LogInformation("Category not found, creating new: {CategoryTitle}", categoryRequest.Title);
                categoryEntity = new Category
                {
                    Id = Guid.CreateVersion7(),
                    Name = categoryRequest.Title,
                    Description = categoryRequest.Description
                };
               await _context.Categories.AddAsync(categoryEntity,cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            book.BookCategories.Add(new BookCategory
            {
                BookId = book.Id,
                CategoryId = categoryEntity.Id
            });
        }

        
        _logger.LogInformation("Saving book to database");
        try
        {
           await _context.Books.AddAsync(book,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Book saved to database successfully with Id: {BookId}", book.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving book {Title} for author {AuthorId}", request.Title, authorId);
            return Result.Failure<BookResponse>(BookErrors.DatabaseSaveError);
        }

        // Save files after successful database save
        try
        {
            var imagePath = Path.Combine(_imagesPath, randomImageName);
            var pdfPath = Path.Combine(_filesPath, randomFileName);

            // Ensure directories exist
            Directory.CreateDirectory(_imagesPath);
            Directory.CreateDirectory(_filesPath);

            _logger.LogInformation("Saving image to: {ImagePath}", imagePath);
            _logger.LogInformation("Saving PDF to: {PdfPath}", pdfPath);

            using (var imageStream = File.Create(imagePath))
                await request.CoverImage.CopyToAsync(imageStream, cancellationToken);

            using (var fileStream = File.Create(pdfPath))
                await request.PdfFile.CopyToAsync(fileStream, cancellationToken);

            _logger.LogInformation("Files saved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving files for book {BookId}. Rolling back database changes.", book.Id);

            // Rollback database changes
            try
            {
                var bookToRemove = await _context.Books
                    .Include(b => b.BookCategories)
                    .FirstAsync(b => b.Id == book.Id, cancellationToken);

                _context.Books.Remove(bookToRemove);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully rolled back database changes for book {BookId}", book.Id);
            }
            catch (Exception rollbackEx)
            {
                _logger.LogError(rollbackEx, "Failed to rollback database changes for book {BookId}", book.Id);
            }

            return Result.Failure<BookResponse>(BookErrors.FileSaveError);
        }
        var savedBook = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstAsync(b => b.Id == book.Id, cancellationToken);

        _logger.LogInformation("Book created successfully with Id: {BookId}", book.Id);
        return Result.Success(savedBook.Adapt<BookResponse>());
    }

    public async Task<Result<PaginatedList<BookResponse>>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Book>()
            .Include(x => x.Author)
            .Where(x =>
            (string.IsNullOrEmpty(filters.SearchTerm) || x.Title.Contains(filters.SearchTerm)) &&
            (string.IsNullOrEmpty(filters.FilterBy) ||
            (filters.FilterBy == SubscriptionType.VIP && x.IsVIP) ||
            (filters.FilterBy == SubscriptionType.Free && !x.IsVIP)))
            .ProjectToType<BookResponse>()
            .AsNoTracking();

        var response = await PaginatedList<BookResponse>.CreateAsync(query, filters.PageNumber, filters.PageSize,cancellationToken);
        return Result.Success(response);
    }

}
