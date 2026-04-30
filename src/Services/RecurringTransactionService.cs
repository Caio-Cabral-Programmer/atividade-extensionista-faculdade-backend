using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class RecurringTransactionService(
    IRecurringTransactionRepository repository) : IRecurringTransactionService
{
    public async Task<List<RecurringTransactionResponse>> GetAllAsync(Guid userId, CancellationToken ct)
    {
        var items = await repository.GetAllByUserAsync(userId, ct);
        return items.Select(ToResponse).ToList();
    }

    public async Task<RecurringTransactionResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var item = await repository.GetByIdAndUserAsync(id, userId, ct)
            ?? throw new NotFoundException("Transação recorrente não encontrada.");

        return ToResponse(item);
    }

    public async Task<RecurringTransactionResponse> CreateAsync(
        CreateRecurringTransactionRequest request, Guid userId, CancellationToken ct)
    {
        var recurring = new RecurringTransaction
        {
            UserId = userId,
            AccountId = request.AccountId,
            CategoryId = request.CategoryId,
            Type = (TransactionType)request.Type,
            Amount = request.Amount,
            Description = request.Description,
            Frequency = (RecurrenceFrequency)request.Frequency,
            DayOfMonth = request.DayOfMonth,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true
        };

        await repository.AddAsync(recurring, ct);

        var created = await repository.GetByIdAndUserAsync(recurring.RecurringTransactionId, userId, ct);
        return ToResponse(created!);
    }

    public async Task<RecurringTransactionResponse> UpdateAsync(
        Guid id, UpdateRecurringTransactionRequest request, Guid userId, CancellationToken ct)
    {
        var recurring = await repository.GetByIdAndUserAsync(id, userId, ct)
            ?? throw new NotFoundException("Transação recorrente não encontrada.");

        recurring.Amount = request.Amount;
        recurring.AccountId = request.AccountId;
        recurring.CategoryId = request.CategoryId;
        recurring.Description = request.Description;
        recurring.Frequency = (RecurrenceFrequency)request.Frequency;
        recurring.DayOfMonth = request.DayOfMonth;
        recurring.EndDate = request.EndDate;
        recurring.IsActive = request.IsActive;

        await repository.UpdateAsync(recurring, ct);

        var updated = await repository.GetByIdAndUserAsync(id, userId, ct);
        return ToResponse(updated!);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var recurring = await repository.GetByIdAndUserAsync(id, userId, ct)
            ?? throw new NotFoundException("Transação recorrente não encontrada.");

        await repository.DeleteAsync(recurring, ct);
    }

    private static RecurringTransactionResponse ToResponse(RecurringTransaction r) =>
        new(
            r.RecurringTransactionId,
            (int)r.Type,
            r.Type switch
            {
                TransactionType.Income => "Receita",
                TransactionType.Expense => "Despesa",
                _ => "Transferência"
            },
            r.Amount,
            r.Description,
            (int)r.Frequency,
            r.Frequency switch
            {
                RecurrenceFrequency.Daily => "Diária",
                RecurrenceFrequency.Weekly => "Semanal",
                RecurrenceFrequency.Monthly => "Mensal",
                RecurrenceFrequency.Yearly => "Anual",
                _ => r.Frequency.ToString()
            },
            r.DayOfMonth,
            r.StartDate,
            r.EndDate,
            r.IsActive,
            r.AccountId,
            r.Account?.Name ?? string.Empty,
            r.CategoryId,
            r.Category?.Name ?? string.Empty);
}
