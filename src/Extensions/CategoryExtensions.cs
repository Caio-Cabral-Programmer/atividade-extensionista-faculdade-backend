using AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;

namespace AtividadeExtensionistaFaculdadeBackend.Extensions;

public static class CategoryExtensions
{
    public static CategoryResponse ToResponse(this Category category) =>
        new(
            category.CategoryId,
            category.Name,
            (int)category.Type,
            category.Type == CategoryType.Income ? "Receita" : "Despesa",
            category.Icon,
            category.Color,
            category.UserId == null,
            category.ParentCategoryId,
            category.ParentCategory?.Name,
            category.SubCategories.Select(sc => sc.ToResponse()).ToList());
}
