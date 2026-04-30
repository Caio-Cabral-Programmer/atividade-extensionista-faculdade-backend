using AtividadeExtensionistaFaculdadeBackend.DTOs.Budgets;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class BudgetService(IBudgetRepository repository) : IBudgetService
{
    public async Task<List<BudgetResponse>> GetAllAsync(Guid userId, int year, int month, CancellationToken ct)
    {
        var budgets = await repository.GetAllByUserAsync(userId, year, month, ct);
        var responses = new List<BudgetResponse>();

        foreach (var budget in budgets)
        {
            var spent = await repository.GetSpentAmountAsync(userId, budget.CategoryId, year, month, ct);
            responses.Add(ToResponse(budget, spent));
        }

        return responses;
    }

    public async Task<BudgetResponse> CreateAsync(CreateBudgetRequest request, Guid userId, CancellationToken ct)
    {
        if (await repository.ExistsAsync(userId, request.CategoryId, request.Year, request.Month, ct))
            throw new BusinessRuleException("Já existe um orçamento para esta categoria neste mês.");

        var budget = new Budget
        {
            UserId = userId,
            CategoryId = request.CategoryId,
            Year = request.Year,
            Month = request.Month,
            LimitAmount = request.LimitAmount
        };

        await repository.AddAsync(budget, ct);

        var created = await repository.GetByIdAndUserAsync(budget.BudgetId, userId, ct);
        return ToResponse(created!, 0);
    }

    public async Task<BudgetResponse> UpdateAsync(Guid budgetId, UpdateBudgetRequest request, Guid userId, CancellationToken ct)
    {
        var budget = await repository.GetByIdAndUserAsync(budgetId, userId, ct)
            ?? throw new NotFoundException("Orçamento não encontrado.");

        budget.LimitAmount = request.LimitAmount;
        await repository.UpdateAsync(budget, ct);

        var spent = await repository.GetSpentAmountAsync(userId, budget.CategoryId, budget.Year, budget.Month, ct);
        return ToResponse(budget, spent);
    }

    public async Task DeleteAsync(Guid budgetId, Guid userId, CancellationToken ct)
    {
        var budget = await repository.GetByIdAndUserAsync(budgetId, userId, ct)
            ?? throw new NotFoundException("Orçamento não encontrado.");

        await repository.DeleteAsync(budget, ct);
    }

    private static BudgetResponse ToResponse(Budget budget, decimal spent) =>
        new(
            budget.BudgetId,
            budget.CategoryId,
            budget.Category?.Name ?? string.Empty,
            budget.Category?.Icon ?? string.Empty,
            budget.Category?.Color ?? string.Empty,
            budget.Year,
            budget.Month,
            budget.LimitAmount,
            spent,
            budget.LimitAmount - spent,
            budget.LimitAmount > 0 ? Math.Round((double)(spent / budget.LimitAmount) * 100, 1) : 0);
}
