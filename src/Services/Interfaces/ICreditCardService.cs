using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface ICreditCardService
{
    Task<List<CreditCardResponse>> GetAllAsync(Guid userId, CancellationToken ct);
    Task<CreditCardResponse> GetByIdAsync(Guid creditCardId, Guid userId, CancellationToken ct);
    Task<CreditCardResponse> CreateAsync(CreateCreditCardRequest request, Guid userId, CancellationToken ct);
    Task<CreditCardResponse> UpdateAsync(Guid creditCardId, UpdateCreditCardRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid creditCardId, Guid userId, CancellationToken ct);
}
