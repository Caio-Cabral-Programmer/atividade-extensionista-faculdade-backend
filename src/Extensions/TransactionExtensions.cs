using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;

namespace AtividadeExtensionistaFaculdadeBackend.Extensions;

public static class TransactionExtensions
{
    public static TransactionResponse ToResponse(this Transaction t) =>
        new(
            t.TransactionId,
            t.Type.ToString(),
            t.Type switch
            {
                TransactionType.Income => "Receita",
                TransactionType.Expense => "Despesa",
                TransactionType.Transfer => "Transferência",
                _ => t.Type.ToString()
            },
            t.Amount,
            t.Date,
            t.Description,
            t.Status.ToString(),
            t.Status == TransactionStatus.Paid ? "Pago" : "Pendente",
            t.IsInstallment,
            t.InstallmentNumber,
            t.TotalInstallments,
            t.AccountId,
            t.Account?.Name ?? string.Empty,
            t.DestinationAccountId,
            t.DestinationAccount?.Name,
            t.CreditCardId,
            t.CreditCard?.Name,
            t.CategoryId,
            t.Category?.Name ?? string.Empty,
            t.Category?.Icon ?? string.Empty,
            t.Category?.Color ?? string.Empty,
            t.TransactionTags.Select(tt => new TagSummary(tt.TagId, tt.Tag.Name, tt.Tag.Color)).ToList(),
            t.CreatedAt);
}
