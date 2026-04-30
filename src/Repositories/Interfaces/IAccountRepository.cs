using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;
using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<List<Account>> GetAllByUserAsync(Guid userId, CancellationToken ct);
    Task<Account?> GetByIdAndUserAsync(Guid accountId, Guid userId, CancellationToken ct);
    Task AddAsync(Account account, CancellationToken ct);
    Task UpdateAsync(Account account, CancellationToken ct);
    Task DeleteAsync(Account account, CancellationToken ct);
    Task<decimal> GetCurrentBalanceAsync(Guid accountId, CancellationToken ct);
}
