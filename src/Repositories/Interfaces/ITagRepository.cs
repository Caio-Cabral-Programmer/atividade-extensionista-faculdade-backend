using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetAllByUserAsync(Guid userId, CancellationToken ct);
    Task<Tag?> GetByIdAndUserAsync(Guid tagId, Guid userId, CancellationToken ct);
    Task AddAsync(Tag tag, CancellationToken ct);
    Task UpdateAsync(Tag tag, CancellationToken ct);
    Task DeleteAsync(Tag tag, CancellationToken ct);
}
