using AtividadeExtensionistaFaculdadeBackend.DTOs.Tags;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface ITagService
{
    Task<List<TagResponse>> GetAllAsync(Guid userId, CancellationToken ct);
    Task<TagResponse> CreateAsync(CreateTagRequest request, Guid userId, CancellationToken ct);
    Task<TagResponse> UpdateAsync(Guid tagId, UpdateTagRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid tagId, Guid userId, CancellationToken ct);
}
