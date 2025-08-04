namespace Bookistry.API.Services;

public class CategoryService(ApplicationDbContext context) : ICategoryService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<CategoryResponse>> CreateAsunc(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var titleExists = await _context.Categories
            .AnyAsync(c => c.Name == request.Title, cancellationToken);
        if (titleExists)
            return Result.Failure<CategoryResponse>(CategoryErrors.DuplicateTitle);
        var category = request.Adapt<Category>();
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        var response = category.Adapt<CategoryResponse>();
        return Result.Success(response);
    }
    public async Task<Result<CategoryResponse>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await GetCategory(id, cancellationToken);
        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound);
        var response = category.Adapt<CategoryResponse>();
        return Result.Success(response);
    }
    public async Task<Result> UpdateAsync(Guid id, CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await GetCategory(id, cancellationToken);
        if (category is null)
            return Result.Failure(CategoryErrors.NotFound);
        var titleExists = await _context.Categories
            .AnyAsync(c => c.Name == request.Title && c.Id != id, cancellationToken);
        if (titleExists)
            return Result.Failure(CategoryErrors.DuplicateTitle);
        category = request.Adapt(category);
        await _context.Categories
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(setter =>
            setter.SetProperty(c => c.Name, category.Name)       
            .SetProperty(c => c.Description, category.Description)
            , cancellationToken);
        return Result.Success();
    }
    private async Task<Category?> GetCategory(Guid id, CancellationToken cancellationToken = default) =>
         await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    
    

}
