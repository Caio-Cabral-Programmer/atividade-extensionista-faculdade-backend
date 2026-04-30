using AtividadeExtensionistaFaculdadeBackend.DTOs.Budgets;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface IBudgetService
{
    Task<List<BudgetResponse>> GetAllAsync(Guid userId, int year, int month, CancellationToken ct);
    Task<BudgetResponse> CreateAsync(CreateBudgetRequest request, Guid userId, CancellationToken ct);
    Task<BudgetResponse> UpdateAsync(Guid budgetId, UpdateBudgetRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid budgetId, Guid userId, CancellationToken ct);
}
