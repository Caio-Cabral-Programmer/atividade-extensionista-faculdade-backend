using AtividadeExtensionistaFaculdadeBackend.DTOs.Goals;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class FinancialGoalService(IFinancialGoalRepository repository) : IFinancialGoalService
{
    public async Task<List<GoalResponse>> GetAllAsync(Guid userId, CancellationToken ct)
    {
        var goals = await repository.GetAllByUserAsync(userId, ct);
        return goals.Select(ToResponse).ToList();
    }

    public async Task<GoalResponse> GetByIdAsync(Guid goalId, Guid userId, CancellationToken ct)
    {
        var goal = await repository.GetByIdAndUserAsync(goalId, userId, ct)
            ?? throw new NotFoundException("Meta financeira não encontrada.");

        return ToResponse(goal);
    }

    public async Task<GoalResponse> CreateAsync(CreateGoalRequest request, Guid userId, CancellationToken ct)
    {
        var goal = new FinancialGoal
        {
            UserId = userId,
            Name = request.Name,
            TargetAmount = request.TargetAmount,
            CurrentAmount = 0,
            Deadline = request.Deadline,
            Description = request.Description,
            Icon = request.Icon
        };

        await repository.AddAsync(goal, ct);
        return ToResponse(goal);
    }

    public async Task<GoalResponse> UpdateAsync(Guid goalId, UpdateGoalRequest request, Guid userId, CancellationToken ct)
    {
        var goal = await repository.GetByIdAndUserAsync(goalId, userId, ct)
            ?? throw new NotFoundException("Meta financeira não encontrada.");

        goal.Name = request.Name;
        goal.TargetAmount = request.TargetAmount;
        goal.Deadline = request.Deadline;
        goal.Description = request.Description;
        goal.Icon = request.Icon;

        await repository.UpdateAsync(goal, ct);
        return ToResponse(goal);
    }

    public async Task<GoalResponse> DepositAsync(Guid goalId, GoalDepositWithdrawRequest request, Guid userId, CancellationToken ct)
    {
        if (request.Amount <= 0)
            throw new BusinessRuleException("O valor do aporte deve ser maior que zero.");

        var goal = await repository.GetByIdAndUserAsync(goalId, userId, ct)
            ?? throw new NotFoundException("Meta financeira não encontrada.");

        goal.CurrentAmount += request.Amount;
        await repository.UpdateAsync(goal, ct);
        return ToResponse(goal);
    }

    public async Task<GoalResponse> WithdrawAsync(Guid goalId, GoalDepositWithdrawRequest request, Guid userId, CancellationToken ct)
    {
        if (request.Amount <= 0)
            throw new BusinessRuleException("O valor de retirada deve ser maior que zero.");

        var goal = await repository.GetByIdAndUserAsync(goalId, userId, ct)
            ?? throw new NotFoundException("Meta financeira não encontrada.");

        if (request.Amount > goal.CurrentAmount)
            throw new BusinessRuleException("Valor de retirada maior do que o saldo da meta.");

        goal.CurrentAmount -= request.Amount;
        await repository.UpdateAsync(goal, ct);
        return ToResponse(goal);
    }

    public async Task DeleteAsync(Guid goalId, Guid userId, CancellationToken ct)
    {
        var goal = await repository.GetByIdAndUserAsync(goalId, userId, ct)
            ?? throw new NotFoundException("Meta financeira não encontrada.");

        await repository.DeleteAsync(goal, ct);
    }

    private static GoalResponse ToResponse(FinancialGoal g) =>
        new(
            g.FinancialGoalId,
            g.Name,
            g.TargetAmount,
            g.CurrentAmount,
            g.TargetAmount - g.CurrentAmount,
            g.TargetAmount > 0 ? Math.Round((double)(g.CurrentAmount / g.TargetAmount) * 100, 1) : 0,
            g.Deadline,
            g.Description,
            g.Icon,
            g.CurrentAmount >= g.TargetAmount,
            g.CreatedAt);
}
