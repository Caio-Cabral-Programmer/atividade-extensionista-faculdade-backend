namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;

public sealed record CreateCategoryRequest(
    string Name,
    int Type,
    string Icon,
    string Color,
    Guid? ParentCategoryId);

public sealed record UpdateCategoryRequest(
    string Name,
    string Icon,
    string Color);

public sealed record CategoryResponse(
    Guid CategoryId,
    string Name,
    int Type,
    string TypeName,
    string Icon,
    string Color,
    bool IsSystem,
    Guid? ParentCategoryId,
    string? ParentCategoryName,
    List<CategoryResponse> SubCategories);
