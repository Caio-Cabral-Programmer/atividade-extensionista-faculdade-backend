using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class AccountRepository(AppDbContext db) : IAccountRepository
{
    public Task<List<Account>> GetAllByUserAsync(Guid userId, CancellationToken ct) =>
        db.Accounts.Where(a => a.UserId == userId).OrderBy(a => a.Name).ToListAsync(ct);

    public Task<Account?> GetByIdAndUserAsync(Guid accountId, Guid userId, CancellationToken ct) =>
        db.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId && a.UserId == userId, ct);

    public async Task AddAsync(Account account, CancellationToken ct)
    {
        await db.Accounts.AddAsync(account, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Account account, CancellationToken ct)
    {
        db.Accounts.Update(account);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Account account, CancellationToken ct)
    {
        db.Accounts.Remove(account);
        await db.SaveChangesAsync(ct);
    }

    public async Task<decimal> GetCurrentBalanceAsync(Guid accountId, CancellationToken ct)
    {
        var account = await db.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId, ct);
        if (account is null) return 0;

        var income = await db.Transactions
            .Where(t => t.AccountId == accountId
                     && t.Type == TransactionType.Income
                     && t.Status == TransactionStatus.Paid)
            .SumAsync(t => t.Amount, ct);

        var expense = await db.Transactions
            .Where(t => t.AccountId == accountId
                     && t.Type == TransactionType.Expense
                     && t.Status == TransactionStatus.Paid)
            .SumAsync(t => t.Amount, ct);

        var transfersOut = await db.Transactions
            .Where(t => t.AccountId == accountId
                     && t.Type == TransactionType.Transfer
                     && t.Status == TransactionStatus.Paid)
            .SumAsync(t => t.Amount, ct);

        var transfersIn = await db.Transactions
            .Where(t => t.DestinationAccountId == accountId
                     && t.Type == TransactionType.Transfer
                     && t.Status == TransactionStatus.Paid)
            .SumAsync(t => t.Amount, ct);

        return account.InitialBalance + income - expense - transfersOut + transfersIn;
    }
}
