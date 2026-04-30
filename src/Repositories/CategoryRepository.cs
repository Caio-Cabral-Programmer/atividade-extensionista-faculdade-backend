using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public Task<List<Category>> GetAllForUserAsync(Guid userId, CancellationToken ct) =>
        db.Categories
            .Include(c => c.SubCategories)
            .Where(c => (c.UserId == userId || c.UserId == null) && c.ParentCategoryId == null)
            .OrderBy(c => c.Type).ThenBy(c => c.Name)
            .ToListAsync(ct);

    public Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken ct) =>
        db.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId, ct);

    public Task<Category?> GetByIdAndUserAsync(Guid categoryId, Guid userId, CancellationToken ct) =>
        db.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.UserId == userId, ct);

    public async Task AddAsync(Category category, CancellationToken ct)
    {
        await db.Categories.AddAsync(category, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Category category, CancellationToken ct)
    {
        db.Categories.Update(category);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Category category, CancellationToken ct)
    {
        db.Categories.Remove(category);
        await db.SaveChangesAsync(ct);
    }
}
