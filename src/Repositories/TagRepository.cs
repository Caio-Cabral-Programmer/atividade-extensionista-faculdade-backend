using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class TagRepository(AppDbContext db) : ITagRepository
{
    public Task<List<Tag>> GetAllByUserAsync(Guid userId, CancellationToken ct) =>
        db.Tags.Where(t => t.UserId == userId).OrderBy(t => t.Name).ToListAsync(ct);

    public Task<Tag?> GetByIdAndUserAsync(Guid tagId, Guid userId, CancellationToken ct) =>
        db.Tags.FirstOrDefaultAsync(t => t.TagId == tagId && t.UserId == userId, ct);

    public async Task AddAsync(Tag tag, CancellationToken ct)
    {
        await db.Tags.AddAsync(tag, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Tag tag, CancellationToken ct)
    {
        db.Tags.Update(tag);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Tag tag, CancellationToken ct)
    {
        db.Tags.Remove(tag);
        await db.SaveChangesAsync(ct);
    }
}
