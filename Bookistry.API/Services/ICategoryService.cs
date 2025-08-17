namespace Bookistry.API.Services;  

public interface ICategoryService
{
    Task<Result<CategoryResponse>> CreateAsunc(CategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, CategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result> RestoreAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
}
