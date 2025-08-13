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

        if (await _context.Books.AnyAsync(x => x.Title == request.Title, cancellationToken))
            return Result.Failure<BookResponse>(BookErrors.DublicatedTitle);

        var randomFileName = Path.GetRandomFileName();
        var randomImageName = Path.GetRandomFileName();

        _logger.LogInformation("Generated file names: Pdf={PdfFile}, Image={ImageFile}", randomFileName, randomImageName);

        var book = new Book
        {
            Title = request.Title,
            Description = request.Description,
            IsVIP = request.IsVIP,
            PageCount = request.PageCount,
            AuthorId = authorId,
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

        _context.Books.Add(book);

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
                    Name = categoryRequest.Title,
                    Description = categoryRequest.Description
                };

                _context.Categories.Add(categoryEntity);
            }

            var bookCategory = new BookCategory
            {
                BookId = book.Id,
                CategoryId = categoryEntity.Id,
                Book = book,
                Category = categoryEntity
            };
            _context.Set<BookCategory>().Add(bookCategory);
            book.BookCategories.Add(bookCategory);
        }

        _logger.LogInformation("Saving book and related data to database");
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Book saved to database successfully with Id: {BookId}", book.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving book {Title} for author {AuthorId}", request.Title, authorId);
            return Result.Failure<BookResponse>(BookErrors.DatabaseSaveError);
        }

        try
        {
            var imagePath = Path.Combine(_imagesPath, randomImageName);
            var pdfPath = Path.Combine(_filesPath, randomFileName);

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

        _logger.LogInformation("Book created successfully with Id: {BookId}", book.Id);
       var response = await _context.Books
            .Where(b => b.Id == book.Id && b.AuthorId == authorId)
            .AsNoTracking()
            .Select(b => new BookResponse(
                b.Id,
                b.Title,
                b.Description,
                $"{_imagesPath}/{b.CoverImageUpload.StoredFileName}",
                b.PublishedOn,
                b.IsVIP,
                b.AverageRating,
                b.PageCount,
                $"{b.Author.FirstName} {b.Author.LastName}"
            ))
            .FirstOrDefaultAsync(cancellationToken);
        return Result.Success(response!);
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

    public async Task<Result<BookDetailsResponse>> GetAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var query = await _context.Books
                   .Where(b => b.Id == bookId)
                   .Select(b => new BookDetailsResponse(
                       b.Id,
                       b.Title,
                       b.Description,
                       $"{_imagesPath}/{b.CoverImageUpload.StoredFileName}",
                       $"{_filesPath}/{b.PdfFileUpload.StoredFileName}",
                       b.PublishedOn,
                       b.IsVIP ? SubscriptionType.VIP : SubscriptionType.Free,
                       b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0,
                       b.ViewCount,
                       b.DownloadCount,
                       b.PageCount,
                       b.Author.FirstName + " " + b.Author.LastName,
                       b.BookCategories.Select(bc => new CategoryResponse(
                           bc.Category.Id,
                           bc.Category.Name,
                           bc.Category.Description
                       )),
                       b.Reviews.Select(r => new ReviewResponse(
                           r.Book.Author.UserName!,
                           r.Rating,
                           r.Comment,
                           r.CreatedOn
                       ))
                   ))
                   .FirstOrDefaultAsync(cancellationToken);

        if (query is null)
            return Result.Failure<BookDetailsResponse>(BookErrors.NotFound);
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);
        if (book is not null)
        {
            book.ViewCount++;
            await _context.SaveChangesAsync(cancellationToken);
        }
        _logger.LogInformation("Retrieved book details for bookId: {BookId}", bookId);
        return Result.Success(query);
    }
    public async Task<Result<PdfDownloadResponse>> DownloadAsync(Guid bookId,string userId,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF download for bookId: {BookId} by userId: {UserId}", bookId, userId);

        var book = await _context.Books
            .Include(b => b.PdfFileUpload)
            .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);

        if (book is null)
        {
            _logger.LogWarning("Book with Id {BookId} not found", bookId);
            return Result.Failure<PdfDownloadResponse>(BookErrors.NotFound);
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<PdfDownloadResponse>(UserErrors.NotFound);

        if (book.IsVIP && !user.IsVIP)
        {
            _logger.LogWarning("User {UserId} attempted to download VIP book {BookId} without VIP access", userId, bookId);
            return Result.Failure<PdfDownloadResponse>(BookErrors.VipRequired);
        }


        var path = Path.Combine(_filesPath, book.PdfFileUpload.StoredFileName);

        if (!File.Exists(path))
        {
            _logger.LogError("PDF file not found on disk for bookId: {BookId}, path: {Path}", bookId, path);
            return Result.Failure<PdfDownloadResponse>(BookErrors.NotFound);
        }

        book.DownloadCount++;
        await _context.SaveChangesAsync(cancellationToken);

        MemoryStream memoryStream = new();
        await using (FileStream fileStream = new(path, FileMode.Open, FileAccess.Read))
        {
            await fileStream.CopyToAsync(memoryStream, cancellationToken);
        }

        memoryStream.Position = 0;

        var response = new PdfDownloadResponse(
            memoryStream.ToArray(),
            book.PdfFileUpload.ContentType,
            book.PdfFileUpload.FileName
        );

        _logger.LogInformation("PDF download completed successfully for bookId: {BookId} by userId: {UserId}", bookId, userId);
        return Result.Success(response);
    }

}
