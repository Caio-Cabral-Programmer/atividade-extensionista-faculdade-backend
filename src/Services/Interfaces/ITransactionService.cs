using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface ITransactionService
{
    Task<PagedResponse<TransactionResponse>> GetAllAsync(Guid userId, TransactionFilterRequest filter, CancellationToken ct);
    Task<TransactionResponse> GetByIdAsync(Guid transactionId, Guid userId, CancellationToken ct);
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, Guid userId, CancellationToken ct);
    Task<TransactionResponse> UpdateAsync(Guid transactionId, UpdateTransactionRequest request, Guid userId, CancellationToken ct);
    Task<TransactionResponse> UpdateStatusAsync(Guid transactionId, UpdateTransactionStatusRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid transactionId, Guid userId, CancellationToken ct);
}
