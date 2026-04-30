using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface ICreditCardRepository
{
    Task<List<CreditCard>> GetAllByUserAsync(Guid userId, CancellationToken ct);
    Task<CreditCard?> GetByIdAndUserAsync(Guid creditCardId, Guid userId, CancellationToken ct);
    Task AddAsync(CreditCard creditCard, CancellationToken ct);
    Task UpdateAsync(CreditCard creditCard, CancellationToken ct);
    Task DeleteAsync(CreditCard creditCard, CancellationToken ct);
    Task<decimal> GetCurrentInvoiceTotalAsync(Guid creditCardId, CancellationToken ct);
}
