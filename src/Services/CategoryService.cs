using AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Extensions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class CategoryService(ICategoryRepository repository) : ICategoryService
{
    public async Task<List<CategoryResponse>> GetAllAsync(Guid userId, CancellationToken ct)
    {
        var categories = await repository.GetAllForUserAsync(userId, ct);
        return categories.Select(c => c.ToResponse()).ToList();
    }

    public async Task<CategoryResponse> GetByIdAsync(Guid categoryId, Guid userId, CancellationToken ct)
    {
        var category = await repository.GetByIdAsync(categoryId, ct);

        if (category is null || (category.UserId != null && category.UserId != userId))
            throw new NotFoundException("Categoria não encontrada.");

        return category.ToResponse();
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, Guid userId, CancellationToken ct)
    {
        Category? parent = null;
        if (request.ParentCategoryId.HasValue)
        {
            parent = await repository.GetByIdAsync(request.ParentCategoryId.Value, ct);
            if (parent is null || (parent.UserId != null && parent.UserId != userId))
                throw new NotFoundException("Categoria pai não encontrada.");
        }

        var category = new Category
        {
            UserId = userId,
            Name = request.Name,
            Type = (CategoryType)request.Type,
            Icon = request.Icon,
            Color = request.Color,
            ParentCategoryId = request.ParentCategoryId
        };

        await repository.AddAsync(category, ct);
        category.ParentCategory = parent;
        return category.ToResponse();
    }

    public async Task<CategoryResponse> UpdateAsync(Guid categoryId, UpdateCategoryRequest request, Guid userId, CancellationToken ct)
    {
        var category = await repository.GetByIdAndUserAsync(categoryId, userId, ct)
            ?? throw new NotFoundException("Categoria não encontrada ou não pode ser editada.");

        category.Name = request.Name;
        category.Icon = request.Icon;
        category.Color = request.Color;

        await repository.UpdateAsync(category, ct);
        return category.ToResponse();
    }

    public async Task DeleteAsync(Guid categoryId, Guid userId, CancellationToken ct)
    {
        var category = await repository.GetByIdAndUserAsync(categoryId, userId, ct)
            ?? throw new NotFoundException("Categoria não encontrada ou não pode ser excluída.");

        await repository.DeleteAsync(category, ct);
    }
}
