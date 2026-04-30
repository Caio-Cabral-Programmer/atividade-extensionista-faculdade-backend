using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface IRecurringTransactionRepository
{
    Task<List<RecurringTransaction>> GetAllByUserAsync(Guid userId, CancellationToken ct);
    Task<RecurringTransaction?> GetByIdAndUserAsync(Guid id, Guid userId, CancellationToken ct);
    Task AddAsync(RecurringTransaction recurring, CancellationToken ct);
    Task UpdateAsync(RecurringTransaction recurring, CancellationToken ct);
    Task DeleteAsync(RecurringTransaction recurring, CancellationToken ct);
    Task EnsureOccurrencesGeneratedAsync(Guid userId, DateOnly? from, DateOnly? to, CancellationToken ct);
}
