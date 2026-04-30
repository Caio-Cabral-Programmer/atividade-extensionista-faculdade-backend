using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface IBudgetRepository
{
    Task<List<Budget>> GetAllByUserAsync(Guid userId, int year, int month, CancellationToken ct);
    Task<Budget?> GetByIdAndUserAsync(Guid budgetId, Guid userId, CancellationToken ct);
    Task<bool> ExistsAsync(Guid userId, Guid categoryId, int year, int month, CancellationToken ct);
    Task AddAsync(Budget budget, CancellationToken ct);
    Task UpdateAsync(Budget budget, CancellationToken ct);
    Task DeleteAsync(Budget budget, CancellationToken ct);
    Task<decimal> GetSpentAmountAsync(Guid userId, Guid categoryId, int year, int month, CancellationToken ct);
}
