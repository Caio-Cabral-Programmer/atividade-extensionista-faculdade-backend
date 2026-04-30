namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;

public sealed record CreateCreditCardRequest(
    string Name,
    decimal CreditLimit,
    int ClosingDay,
    int DueDay,
    string Color);

public sealed record UpdateCreditCardRequest(
    string Name,
    decimal CreditLimit,
    int ClosingDay,
    int DueDay,
    string Color,
    bool IsActive);

public sealed record CreditCardResponse(
    Guid CreditCardId,
    string Name,
    decimal CreditLimit,
    decimal CurrentInvoiceTotal,
    decimal AvailableLimit,
    int ClosingDay,
    int DueDay,
    string Color,
    bool IsActive,
    DateTime CreatedAt);
