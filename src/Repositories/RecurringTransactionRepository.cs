using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class RecurringTransactionRepository(AppDbContext db) : IRecurringTransactionRepository
{
    public Task<List<RecurringTransaction>> GetAllByUserAsync(Guid userId, CancellationToken ct) =>
        db.RecurringTransactions
            .Include(r => r.Account)
            .Include(r => r.Category)
            .Where(r => r.UserId == userId)
            .OrderBy(r => r.StartDate)
            .ToListAsync(ct);

    public Task<RecurringTransaction?> GetByIdAndUserAsync(Guid id, Guid userId, CancellationToken ct) =>
        db.RecurringTransactions
            .Include(r => r.Account)
            .Include(r => r.Category)
            .FirstOrDefaultAsync(r => r.RecurringTransactionId == id && r.UserId == userId, ct);

    public async Task AddAsync(RecurringTransaction recurring, CancellationToken ct)
    {
        await db.RecurringTransactions.AddAsync(recurring, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(RecurringTransaction recurring, CancellationToken ct)
    {
        db.RecurringTransactions.Update(recurring);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(RecurringTransaction recurring, CancellationToken ct)
    {
        db.RecurringTransactions.Remove(recurring);
        await db.SaveChangesAsync(ct);
    }

    public async Task EnsureOccurrencesGeneratedAsync(
        Guid userId, DateOnly? from, DateOnly? to, CancellationToken ct)
    {
        var periodEnd = to ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1));
        var periodStart = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));

        var recurrings = await db.RecurringTransactions
            .Where(r => r.UserId == userId && r.IsActive
                     && r.StartDate <= periodEnd
                     && (r.EndDate == null || r.EndDate >= periodStart))
            .ToListAsync(ct);

        var newTransactions = new List<Transaction>();

        foreach (var recurring in recurrings)
        {
            var occurrences = GetOccurrencesInPeriod(recurring, periodStart, periodEnd);

            foreach (var occurrenceDate in occurrences)
            {
                var alreadyExists = await db.Transactions.AnyAsync(
                    t => t.RecurringTransactionId == recurring.RecurringTransactionId
                      && t.Date == occurrenceDate, ct);

                if (!alreadyExists)
                {
                    newTransactions.Add(new Transaction
                    {
                        UserId = recurring.UserId,
                        AccountId = recurring.AccountId,
                        CategoryId = recurring.CategoryId,
                        Type = recurring.Type,
                        Amount = recurring.Amount,
                        Date = occurrenceDate,
                        Description = recurring.Description,
                        Status = TransactionStatus.Pending,
                        RecurringTransactionId = recurring.RecurringTransactionId
                    });
                }
            }
        }

        if (newTransactions.Count > 0)
        {
            await db.Transactions.AddRangeAsync(newTransactions, ct);
            await db.SaveChangesAsync(ct);
        }
    }

    private static IEnumerable<DateOnly> GetOccurrencesInPeriod(
        RecurringTransaction recurring, DateOnly from, DateOnly to)
    {
        var current = recurring.StartDate;
        var results = new List<DateOnly>();

        while (current <= to)
        {
            if (current >= from)
                results.Add(current);

            current = recurring.Frequency switch
            {
                RecurrenceFrequency.Daily => current.AddDays(1),
                RecurrenceFrequency.Weekly => current.AddDays(7),
                RecurrenceFrequency.Monthly => current.AddMonths(1),
                RecurrenceFrequency.Yearly => current.AddYears(1),
                _ => current.AddMonths(1)
            };

            if (recurring.EndDate.HasValue && current > recurring.EndDate.Value)
                break;
        }

        return results;
    }
}
