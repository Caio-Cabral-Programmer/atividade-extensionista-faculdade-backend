using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;

namespace AtividadeExtensionistaFaculdadeBackend.Extensions;

public static class AccountExtensions
{
    public static AccountResponse ToResponse(this Account account, decimal currentBalance) =>
        new(
            account.AccountId,
            account.Name,
            (int)account.Type,
            account.Type switch
            {
                AccountType.CheckingAccount => "Conta Corrente",
                AccountType.Savings => "Poupança",
                AccountType.Cash => "Dinheiro",
                AccountType.Investment => "Investimento",
                _ => account.Type.ToString()
            },
            account.InitialBalance,
            currentBalance,
            account.Color,
            account.Icon,
            account.IsActive,
            account.CreatedAt);
}

public static class CreditCardExtensions
{
    public static CreditCardResponse ToResponse(this CreditCard card, decimal invoiceTotal) =>
        new(
            card.CreditCardId,
            card.Name,
            card.CreditLimit,
            invoiceTotal,
            card.CreditLimit - invoiceTotal,
            card.ClosingDay,
            card.DueDay,
            card.Color,
            card.IsActive,
            card.CreatedAt);
}
