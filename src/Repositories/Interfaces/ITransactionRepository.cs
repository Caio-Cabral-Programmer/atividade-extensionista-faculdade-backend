using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<(List<Transaction> Data, int Total)> GetPagedAsync(Guid userId, TransactionFilterRequest filter, CancellationToken ct);
    Task<Transaction?> GetByIdAndUserAsync(Guid transactionId, Guid userId, CancellationToken ct);
    Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct);
    Task UpdateAsync(Transaction transaction, CancellationToken ct);
    Task DeleteAsync(Transaction transaction, CancellationToken ct);
    Task AddTagsAsync(Guid transactionId, IEnumerable<Guid> tagIds, CancellationToken ct);
    Task AddTagsBatchAsync(IEnumerable<(Guid TransactionId, Guid TagId)> pairs, CancellationToken ct);
    Task RemoveAllTagsAsync(Guid transactionId, CancellationToken ct);
}
