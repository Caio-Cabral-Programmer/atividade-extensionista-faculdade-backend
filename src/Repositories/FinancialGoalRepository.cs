using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class FinancialGoalRepository(AppDbContext db) : IFinancialGoalRepository
{
    public Task<List<FinancialGoal>> GetAllByUserAsync(Guid userId, CancellationToken ct) =>
        db.FinancialGoals.Where(g => g.UserId == userId).OrderBy(g => g.Name).ToListAsync(ct);

    public Task<FinancialGoal?> GetByIdAndUserAsync(Guid goalId, Guid userId, CancellationToken ct) =>
        db.FinancialGoals.FirstOrDefaultAsync(g => g.FinancialGoalId == goalId && g.UserId == userId, ct);

    public async Task AddAsync(FinancialGoal goal, CancellationToken ct)
    {
        await db.FinancialGoals.AddAsync(goal, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(FinancialGoal goal, CancellationToken ct)
    {
        db.FinancialGoals.Update(goal);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(FinancialGoal goal, CancellationToken ct)
    {
        db.FinancialGoals.Remove(goal);
        await db.SaveChangesAsync(ct);
    }
}
