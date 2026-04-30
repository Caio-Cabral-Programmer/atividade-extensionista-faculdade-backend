using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface IFinancialGoalRepository
{
    Task<List<FinancialGoal>> GetAllByUserAsync(Guid userId, CancellationToken ct);
    Task<FinancialGoal?> GetByIdAndUserAsync(Guid goalId, Guid userId, CancellationToken ct);
    Task AddAsync(FinancialGoal goal, CancellationToken ct);
    Task UpdateAsync(FinancialGoal goal, CancellationToken ct);
    Task DeleteAsync(FinancialGoal goal, CancellationToken ct);
}
