using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface IRecurringTransactionService
{
    Task<List<RecurringTransactionResponse>> GetAllAsync(Guid userId, CancellationToken ct);
    Task<RecurringTransactionResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken ct);
    Task<RecurringTransactionResponse> CreateAsync(CreateRecurringTransactionRequest request, Guid userId, CancellationToken ct);
    Task<RecurringTransactionResponse> UpdateAsync(Guid id, UpdateRecurringTransactionRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken ct);
}
