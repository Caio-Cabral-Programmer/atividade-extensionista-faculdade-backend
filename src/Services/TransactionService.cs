using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Extensions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class TransactionService(
    ITransactionRepository repository,
    IRecurringTransactionRepository recurringRepository) : ITransactionService
{
    public async Task<PagedResponse<TransactionResponse>> GetAllAsync(
        Guid userId, TransactionFilterRequest filter, CancellationToken ct)
    {
        // Generate on-demand recurring transaction occurrences before returning data
        await recurringRepository.EnsureOccurrencesGeneratedAsync(userId, filter.DateFrom, filter.DateTo, ct);

        var (data, total) = await repository.GetPagedAsync(userId, filter, ct);
        var responses = data.Select(t => t.ToResponse()).ToList();

        return new PagedResponse<TransactionResponse>(responses, filter.Page, filter.PageSize, total);
    }

    public async Task<TransactionResponse> GetByIdAsync(Guid transactionId, Guid userId, CancellationToken ct)
    {
        var transaction = await repository.GetByIdAndUserAsync(transactionId, userId, ct)
            ?? throw new NotFoundException("Transação não encontrada.");

        return transaction.ToResponse();
    }

    public async Task<TransactionResponse> CreateAsync(
        CreateTransactionRequest request, Guid userId, CancellationToken ct)
    {
        var transactions = new List<Transaction>();

        if (request.IsInstallment && request.TotalInstallments > 1)
        {
            var groupId = Guid.NewGuid();
            var installmentAmount = Math.Round(request.Amount / request.TotalInstallments!.Value, 2);

            for (int i = 1; i <= request.TotalInstallments.Value; i++)
            {
                transactions.Add(BuildTransaction(request, userId, installmentAmount, i, request.TotalInstallments.Value, groupId));
            }
        }
        else if (request.Type == (int)TransactionType.Transfer)
        {
            if (!request.DestinationAccountId.HasValue)
                throw new BusinessRuleException("Conta de destino é obrigatória para transferências.");

            // Outgoing leg
            transactions.Add(BuildTransaction(request, userId, request.Amount));

            // Incoming leg  
            var incoming = BuildTransaction(request, userId, request.Amount);
            incoming.AccountId = request.DestinationAccountId.Value;
            incoming.DestinationAccountId = null;
            transactions.Add(incoming);
        }
        else
        {
            transactions.Add(BuildTransaction(request, userId, request.Amount));
        }

        await repository.AddRangeAsync(transactions, ct);

        if (request.TagIds?.Count > 0)
        {
            var allTagPairs = transactions
                .SelectMany(t => request.TagIds.Select(tagId => (t.TransactionId, tagId)))
                .ToList();
            await repository.AddTagsBatchAsync(allTagPairs, ct);
        }

        var created = await repository.GetByIdAndUserAsync(transactions.First().TransactionId, userId, ct);
        return created!.ToResponse();
    }

    public async Task<TransactionResponse> UpdateAsync(
        Guid transactionId, UpdateTransactionRequest request, Guid userId, CancellationToken ct)
    {
        var transaction = await repository.GetByIdAndUserAsync(transactionId, userId, ct)
            ?? throw new NotFoundException("Transação não encontrada.");

        transaction.Amount = request.Amount;
        transaction.Date = request.Date;
        transaction.AccountId = request.AccountId;
        transaction.CreditCardId = request.CreditCardId;
        transaction.CategoryId = request.CategoryId;
        transaction.Description = request.Description;
        transaction.Status = (TransactionStatus)request.Status;

        await repository.UpdateAsync(transaction, ct);

        if (request.TagIds is not null)
        {
            await repository.RemoveAllTagsAsync(transactionId, ct);
            if (request.TagIds.Count > 0)
                await repository.AddTagsAsync(transactionId, request.TagIds, ct);
        }

        var updated = await repository.GetByIdAndUserAsync(transactionId, userId, ct);
        return updated!.ToResponse();
    }

    public async Task<TransactionResponse> UpdateStatusAsync(
        Guid transactionId, UpdateTransactionStatusRequest request, Guid userId, CancellationToken ct)
    {
        var transaction = await repository.GetByIdAndUserAsync(transactionId, userId, ct)
            ?? throw new NotFoundException("Transação não encontrada.");

        if (!Enum.IsDefined(typeof(TransactionStatus), request.Status))
            throw new BusinessRuleException("Status inválido.");

        transaction.Status = (TransactionStatus)request.Status;
        await repository.UpdateAsync(transaction, ct);

        var updated = await repository.GetByIdAndUserAsync(transactionId, userId, ct);
        return updated!.ToResponse();
    }

    public async Task DeleteAsync(Guid transactionId, Guid userId, CancellationToken ct)
    {
        var transaction = await repository.GetByIdAndUserAsync(transactionId, userId, ct)
            ?? throw new NotFoundException("Transação não encontrada.");

        await repository.DeleteAsync(transaction, ct);
    }

    private static Transaction BuildTransaction(
        CreateTransactionRequest request,
        Guid userId,
        decimal amount,
        int? installmentNumber = null,
        int? totalInstallments = null,
        Guid? installmentGroupId = null)
    {
        var date = installmentNumber.HasValue && installmentNumber > 1
            ? request.Date.AddMonths(installmentNumber.Value - 1)
            : request.Date;

        return new Transaction
        {
            UserId = userId,
            AccountId = request.AccountId,
            DestinationAccountId = request.DestinationAccountId,
            CreditCardId = request.CreditCardId,
            CategoryId = request.CategoryId,
            Type = (TransactionType)request.Type,
            Amount = amount,
            Date = date,
            Description = request.Description,
            Status = (TransactionStatus)request.Status,
            IsInstallment = installmentNumber.HasValue,
            InstallmentNumber = installmentNumber,
            TotalInstallments = totalInstallments,
            InstallmentGroupId = installmentGroupId,
            RecurringTransactionId = request.RecurringTransactionId
        };
    }
}
