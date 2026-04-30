using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Extensions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class AccountService(IAccountRepository repository) : IAccountService
{
    public async Task<List<AccountResponse>> GetAllAsync(Guid userId, CancellationToken ct)
    {
        var accounts = await repository.GetAllByUserAsync(userId, ct);
        var responses = new List<AccountResponse>();

        foreach (var account in accounts)
        {
            var balance = await repository.GetCurrentBalanceAsync(account.AccountId, ct);
            responses.Add(account.ToResponse(balance));
        }

        return responses;
    }

    public async Task<AccountResponse> GetByIdAsync(Guid accountId, Guid userId, CancellationToken ct)
    {
        var account = await repository.GetByIdAndUserAsync(accountId, userId, ct)
            ?? throw new NotFoundException("Conta não encontrada.");

        var balance = await repository.GetCurrentBalanceAsync(account.AccountId, ct);
        return account.ToResponse(balance);
    }

    public async Task<AccountResponse> CreateAsync(CreateAccountRequest request, Guid userId, CancellationToken ct)
    {
        var account = new Account
        {
            UserId = userId,
            Name = request.Name,
            Type = (AccountType)request.Type,
            InitialBalance = request.InitialBalance,
            Color = request.Color,
            Icon = request.Icon
        };

        await repository.AddAsync(account, ct);

        return account.ToResponse(account.InitialBalance);
    }

    public async Task<AccountResponse> UpdateAsync(Guid accountId, UpdateAccountRequest request, Guid userId, CancellationToken ct)
    {
        var account = await repository.GetByIdAndUserAsync(accountId, userId, ct)
            ?? throw new NotFoundException("Conta não encontrada.");

        account.Name = request.Name;
        account.Type = (AccountType)request.Type;
        account.InitialBalance = request.InitialBalance;
        account.Color = request.Color;
        account.Icon = request.Icon;
        account.IsActive = request.IsActive;

        await repository.UpdateAsync(account, ct);

        var balance = await repository.GetCurrentBalanceAsync(account.AccountId, ct);
        return account.ToResponse(balance);
    }

    public async Task DeleteAsync(Guid accountId, Guid userId, CancellationToken ct)
    {
        var account = await repository.GetByIdAndUserAsync(accountId, userId, ct)
            ?? throw new NotFoundException("Conta não encontrada.");

        await repository.DeleteAsync(account, ct);
    }
}
