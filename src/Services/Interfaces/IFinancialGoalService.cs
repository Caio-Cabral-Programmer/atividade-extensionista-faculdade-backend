using AtividadeExtensionistaFaculdadeBackend.DTOs.Goals;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface IFinancialGoalService
{
    Task<List<GoalResponse>> GetAllAsync(Guid userId, CancellationToken ct);
    Task<GoalResponse> GetByIdAsync(Guid goalId, Guid userId, CancellationToken ct);
    Task<GoalResponse> CreateAsync(CreateGoalRequest request, Guid userId, CancellationToken ct);
    Task<GoalResponse> UpdateAsync(Guid goalId, UpdateGoalRequest request, Guid userId, CancellationToken ct);
    Task<GoalResponse> DepositAsync(Guid goalId, GoalDepositWithdrawRequest request, Guid userId, CancellationToken ct);
    Task<GoalResponse> WithdrawAsync(Guid goalId, GoalDepositWithdrawRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid goalId, Guid userId, CancellationToken ct);
}
