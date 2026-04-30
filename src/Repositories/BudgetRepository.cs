using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class BudgetRepository(AppDbContext db) : IBudgetRepository
{
    public Task<List<Budget>> GetAllByUserAsync(Guid userId, int year, int month, CancellationToken ct) =>
        db.Budgets
            .Include(b => b.Category)
            .Where(b => b.UserId == userId && b.Year == year && b.Month == month)
            .ToListAsync(ct);

    public Task<Budget?> GetByIdAndUserAsync(Guid budgetId, Guid userId, CancellationToken ct) =>
        db.Budgets.Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.BudgetId == budgetId && b.UserId == userId, ct);

    public Task<bool> ExistsAsync(Guid userId, Guid categoryId, int year, int month, CancellationToken ct) =>
        db.Budgets.AnyAsync(b => b.UserId == userId && b.CategoryId == categoryId
                              && b.Year == year && b.Month == month, ct);

    public async Task AddAsync(Budget budget, CancellationToken ct)
    {
        await db.Budgets.AddAsync(budget, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Budget budget, CancellationToken ct)
    {
        db.Budgets.Update(budget);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Budget budget, CancellationToken ct)
    {
        db.Budgets.Remove(budget);
        await db.SaveChangesAsync(ct);
    }

    public Task<decimal> GetSpentAmountAsync(Guid userId, Guid categoryId, int year, int month, CancellationToken ct) =>
        db.Transactions
            .Where(t => t.UserId == userId
                     && t.CategoryId == categoryId
                     && t.Type == TransactionType.Expense
                     && t.Status == TransactionStatus.Paid
                     && t.Date.Year == year
                     && t.Date.Month == month)
            .SumAsync(t => t.Amount, ct);
}
