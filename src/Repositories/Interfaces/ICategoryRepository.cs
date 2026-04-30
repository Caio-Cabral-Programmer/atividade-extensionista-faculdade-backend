using AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;
using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllForUserAsync(Guid userId, CancellationToken ct);
    Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken ct);
    Task<Category?> GetByIdAndUserAsync(Guid categoryId, Guid userId, CancellationToken ct);
    Task AddAsync(Category category, CancellationToken ct);
    Task UpdateAsync(Category category, CancellationToken ct);
    Task DeleteAsync(Category category, CancellationToken ct);
}
