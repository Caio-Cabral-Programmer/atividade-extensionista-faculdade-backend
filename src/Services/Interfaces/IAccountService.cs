using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface IAccountService
{
    Task<List<AccountResponse>> GetAllAsync(Guid userId, CancellationToken ct);
    Task<AccountResponse> GetByIdAsync(Guid accountId, Guid userId, CancellationToken ct);
    Task<AccountResponse> CreateAsync(CreateAccountRequest request, Guid userId, CancellationToken ct);
    Task<AccountResponse> UpdateAsync(Guid accountId, UpdateAccountRequest request, Guid userId, CancellationToken ct);
    Task DeleteAsync(Guid accountId, Guid userId, CancellationToken ct);
}
