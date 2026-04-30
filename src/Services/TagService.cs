using AtividadeExtensionistaFaculdadeBackend.DTOs.Tags;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class TagService(ITagRepository repository) : ITagService
{
    public async Task<List<TagResponse>> GetAllAsync(Guid userId, CancellationToken ct)
    {
        var tags = await repository.GetAllByUserAsync(userId, ct);
        return tags.Select(t => new TagResponse(t.TagId, t.Name, t.Color)).ToList();
    }

    public async Task<TagResponse> CreateAsync(CreateTagRequest request, Guid userId, CancellationToken ct)
    {
        var tag = new Tag
        {
            UserId = userId,
            Name = request.Name,
            Color = request.Color
        };

        await repository.AddAsync(tag, ct);
        return new TagResponse(tag.TagId, tag.Name, tag.Color);
    }

    public async Task<TagResponse> UpdateAsync(Guid tagId, UpdateTagRequest request, Guid userId, CancellationToken ct)
    {
        var tag = await repository.GetByIdAndUserAsync(tagId, userId, ct)
            ?? throw new NotFoundException("Etiqueta não encontrada.");

        tag.Name = request.Name;
        tag.Color = request.Color;

        await repository.UpdateAsync(tag, ct);
        return new TagResponse(tag.TagId, tag.Name, tag.Color);
    }

    public async Task DeleteAsync(Guid tagId, Guid userId, CancellationToken ct)
    {
        var tag = await repository.GetByIdAndUserAsync(tagId, userId, ct)
            ?? throw new NotFoundException("Etiqueta não encontrada.");

        await repository.DeleteAsync(tag, ct);
    }
}
