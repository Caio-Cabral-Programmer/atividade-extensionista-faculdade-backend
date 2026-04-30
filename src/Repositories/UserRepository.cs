using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken ct) =>
        db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public Task<User?> GetByEmailConfirmationTokenAsync(string token, CancellationToken ct) =>
        db.Users.FirstOrDefaultAsync(u => u.EmailConfirmationToken == token, ct);

    public Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct) =>
        db.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == token, ct);

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await db.Users.AddAsync(user, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync(ct);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct) =>
        db.Users.AnyAsync(u => u.Email == email.ToLowerInvariant(), ct);
}
