using AtividadeExtensionistaFaculdadeBackend.DTOs.Dashboard;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Repositories;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class DashboardService(
    AppDbContext db,
    IAccountRepository accountRepository) : IDashboardService
{
    private static readonly string[] MonthNames =
    [
        "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
        "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
    ];

    public async Task<DashboardSummaryResponse> GetSummaryAsync(Guid userId, CancellationToken ct)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        var accounts = await accountRepository.GetAllByUserAsync(userId, ct);
        decimal totalBalance = 0;
        foreach (var account in accounts.Where(a => a.IsActive))
            totalBalance += await accountRepository.GetCurrentBalanceAsync(account.AccountId, ct);

        var monthIncome = await db.Transactions
            .Where(t => t.UserId == userId
                     && t.Type == TransactionType.Income
                     && t.Status == TransactionStatus.Paid
                     && t.Date.Year == now.Year && t.Date.Month == now.Month)
            .SumAsync(t => t.Amount, ct);

        var monthExpenses = await db.Transactions
            .Where(t => t.UserId == userId
                     && t.Type == TransactionType.Expense
                     && t.Status == TransactionStatus.Paid
                     && t.Date.Year == now.Year && t.Date.Month == now.Month)
            .SumAsync(t => t.Amount, ct);

        var next7Days = now.AddDays(7);
        var billsDue = await db.Transactions
            .Where(t => t.UserId == userId
                     && t.Status == TransactionStatus.Pending
                     && t.Date >= now && t.Date <= next7Days)
            .SumAsync(t => t.Amount, ct);

        var pendingCount = await db.Transactions
            .CountAsync(t => t.UserId == userId && t.Status == TransactionStatus.Pending
                          && t.Date.Year == now.Year && t.Date.Month == now.Month, ct);

        return new DashboardSummaryResponse(totalBalance, monthIncome, monthExpenses, billsDue, pendingCount);
    }

    public async Task<List<ExpensesByCategoryItem>> GetExpensesByCategoryAsync(
        Guid userId, int year, int month, CancellationToken ct)
    {
        var grouped = await db.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId
                     && t.Type == TransactionType.Expense
                     && t.Status == TransactionStatus.Paid
                     && t.Date.Year == year && t.Date.Month == month)
            .GroupBy(t => new { t.CategoryId, t.Category!.Name, t.Category.Color })
            .Select(g => new { g.Key.CategoryId, g.Key.Name, g.Key.Color, Total = g.Sum(t => t.Amount) })
            .OrderByDescending(x => x.Total)
            .ToListAsync(ct);

        var grandTotal = grouped.Sum(x => x.Total);

        return grouped.Select(x => new ExpensesByCategoryItem(
            x.CategoryId,
            x.Name,
            x.Color,
            x.Total,
            grandTotal > 0 ? Math.Round((double)(x.Total / grandTotal) * 100, 1) : 0)).ToList();
    }

    public async Task<List<MonthlyBalanceItem>> GetBalanceEvolutionAsync(
        Guid userId, int year, CancellationToken ct)
    {
        var result = new List<MonthlyBalanceItem>();

        for (int month = 1; month <= 12; month++)
        {
            var income = await db.Transactions
                .Where(t => t.UserId == userId
                         && t.Type == TransactionType.Income
                         && t.Status == TransactionStatus.Paid
                         && t.Date.Year == year && t.Date.Month == month)
                .SumAsync(t => t.Amount, ct);

            var expenses = await db.Transactions
                .Where(t => t.UserId == userId
                         && t.Type == TransactionType.Expense
                         && t.Status == TransactionStatus.Paid
                         && t.Date.Year == year && t.Date.Month == month)
                .SumAsync(t => t.Amount, ct);

            result.Add(new MonthlyBalanceItem(year, month, MonthNames[month - 1], income, expenses, income - expenses));
        }

        return result;
    }

    public async Task<BalanceProjectionResponse> GetBalanceProjectionAsync(Guid userId, CancellationToken ct)
    {
        var accounts = await accountRepository.GetAllByUserAsync(userId, ct);
        decimal currentBalance = 0;
        foreach (var account in accounts.Where(a => a.IsActive))
            currentBalance += await accountRepository.GetCurrentBalanceAsync(account.AccountId, ct);

        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        var endOfMonth = new DateOnly(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

        var pendingExpenses = await db.Transactions
            .Where(t => t.UserId == userId
                     && t.Type == TransactionType.Expense
                     && t.Status == TransactionStatus.Pending
                     && t.Date >= now && t.Date <= endOfMonth)
            .SumAsync(t => t.Amount, ct);

        var pendingIncome = await db.Transactions
            .Where(t => t.UserId == userId
                     && t.Type == TransactionType.Income
                     && t.Status == TransactionStatus.Pending
                     && t.Date >= now && t.Date <= endOfMonth)
            .SumAsync(t => t.Amount, ct);

        return new BalanceProjectionResponse(
            currentBalance,
            pendingExpenses,
            pendingIncome,
            currentBalance - pendingExpenses + pendingIncome);
    }
}
