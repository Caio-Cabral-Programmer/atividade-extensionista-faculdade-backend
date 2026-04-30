using AtividadeExtensionistaFaculdadeBackend.DTOs.Dashboard;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryResponse> GetSummaryAsync(Guid userId, CancellationToken ct);
    Task<List<ExpensesByCategoryItem>> GetExpensesByCategoryAsync(Guid userId, int year, int month, CancellationToken ct);
    Task<List<MonthlyBalanceItem>> GetBalanceEvolutionAsync(Guid userId, int year, CancellationToken ct);
    Task<BalanceProjectionResponse> GetBalanceProjectionAsync(Guid userId, CancellationToken ct);
}
