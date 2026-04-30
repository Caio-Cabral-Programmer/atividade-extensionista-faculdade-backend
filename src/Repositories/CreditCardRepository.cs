using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class CreditCardRepository(AppDbContext db) : ICreditCardRepository
{
    public Task<List<CreditCard>> GetAllByUserAsync(Guid userId, CancellationToken ct) =>
        db.CreditCards.Where(c => c.UserId == userId).OrderBy(c => c.Name).ToListAsync(ct);

    public Task<CreditCard?> GetByIdAndUserAsync(Guid creditCardId, Guid userId, CancellationToken ct) =>
        db.CreditCards.FirstOrDefaultAsync(c => c.CreditCardId == creditCardId && c.UserId == userId, ct);

    public async Task AddAsync(CreditCard creditCard, CancellationToken ct)
    {
        await db.CreditCards.AddAsync(creditCard, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CreditCard creditCard, CancellationToken ct)
    {
        db.CreditCards.Update(creditCard);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(CreditCard creditCard, CancellationToken ct)
    {
        db.CreditCards.Remove(creditCard);
        await db.SaveChangesAsync(ct);
    }

    public Task<decimal> GetCurrentInvoiceTotalAsync(Guid creditCardId, CancellationToken ct)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        return db.Transactions
            .Where(t => t.CreditCardId == creditCardId
                     && t.Type == TransactionType.Expense
                     && t.Date.Month == now.Month
                     && t.Date.Year == now.Year)
            .SumAsync(t => t.Amount, ct);
    }
}
