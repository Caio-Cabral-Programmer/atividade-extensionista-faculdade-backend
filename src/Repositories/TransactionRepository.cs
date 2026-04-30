using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public async Task<(List<Transaction> Data, int Total)> GetPagedAsync(
        Guid userId, TransactionFilterRequest filter, CancellationToken ct)
    {
        var query = db.Transactions
            .Include(t => t.Account)
            .Include(t => t.DestinationAccount)
            .Include(t => t.CreditCard)
            .Include(t => t.Category)
            .Include(t => t.TransactionTags).ThenInclude(tt => tt.Tag)
            .Where(t => t.UserId == userId);

        if (filter.Type is not null && Enum.TryParse<TransactionType>(filter.Type, out var transactionType))
            query = query.Where(t => t.Type == transactionType);

        if (filter.Status is not null && Enum.TryParse<TransactionStatus>(filter.Status, out var transactionStatus))
            query = query.Where(t => t.Status == transactionStatus);

        if (filter.AccountId.HasValue)
            query = query.Where(t => t.AccountId == filter.AccountId.Value);

        if (filter.CategoryId.HasValue)
            query = query.Where(t => t.CategoryId == filter.CategoryId.Value);

        if (filter.TagId.HasValue)
            query = query.Where(t => t.TransactionTags.Any(tt => tt.TagId == filter.TagId.Value));

        if (filter.DateFrom.HasValue)
            query = query.Where(t => t.Date >= filter.DateFrom.Value);

        if (filter.DateTo.HasValue)
            query = query.Where(t => t.Date <= filter.DateTo.Value);

        var total = await query.CountAsync(ct);

        var data = await query
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return (data, total);
    }

    public Task<Transaction?> GetByIdAndUserAsync(Guid transactionId, Guid userId, CancellationToken ct) =>
        db.Transactions
            .Include(t => t.Account)
            .Include(t => t.DestinationAccount)
            .Include(t => t.CreditCard)
            .Include(t => t.Category)
            .Include(t => t.TransactionTags).ThenInclude(tt => tt.Tag)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.UserId == userId, ct);

    public async Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct)
    {
        await db.Transactions.AddRangeAsync(transactions, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken ct)
    {
        db.Transactions.Update(transaction);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Transaction transaction, CancellationToken ct)
    {
        db.Transactions.Remove(transaction);
        await db.SaveChangesAsync(ct);
    }

    public async Task AddTagsAsync(Guid transactionId, IEnumerable<Guid> tagIds, CancellationToken ct)
    {
        var tags = tagIds.Select(tagId => new TransactionTag
        {
            TransactionId = transactionId,
            TagId = tagId
        });

        await db.TransactionTags.AddRangeAsync(tags, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task AddTagsBatchAsync(IEnumerable<(Guid TransactionId, Guid TagId)> pairs, CancellationToken ct)
    {
        var tags = pairs.Select(p => new TransactionTag
        {
            TransactionId = p.TransactionId,
            TagId = p.TagId
        });

        await db.TransactionTags.AddRangeAsync(tags, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAllTagsAsync(Guid transactionId, CancellationToken ct)
    {
        await db.TransactionTags
            .Where(tt => tt.TransactionId == transactionId)
            .ExecuteDeleteAsync(ct);
    }
}
