using AtividadeExtensionistaFaculdadeBackend.Entities;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByEmailConfirmationTokenAsync(string token, CancellationToken ct);
    Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
}
