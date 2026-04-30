using AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync(Guid userId, CancellationToken ct);
    Task<CategoryResponse> GetByIdAsync(Guid categoryId, Guid userId, CancellationToken ct);
    Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, Guid userId, CancellationToken ct);
    Task<CategoryResponse> UpdateAsync(Guid categoryId, UpdateCategoryRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid categoryId, Guid userId, CancellationToken ct);
}
