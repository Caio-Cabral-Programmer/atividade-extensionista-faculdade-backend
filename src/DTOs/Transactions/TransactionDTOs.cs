namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;

public sealed record CreateTransactionRequest(
    string Type,
    decimal Amount,
    DateOnly Date,
    Guid AccountId,
    Guid? DestinationAccountId,
    Guid? CreditCardId,
    Guid CategoryId,
    string? Description,
    string Status,
    bool IsInstallment,
    int? TotalInstallments,
    bool IsRecurring,
    Guid? RecurringTransactionId,
    List<Guid>? TagIds);

public sealed record UpdateTransactionRequest(
    decimal Amount,
    DateOnly Date,
    Guid AccountId,
    Guid? CreditCardId,
    Guid CategoryId,
    string? Description,
    string Status,
    List<Guid>? TagIds);

public sealed record UpdateTransactionStatusRequest(string Status);

public sealed record TransactionFilterRequest(
    string? Type,
    string? Status,
    Guid? AccountId,
    Guid? CategoryId,
    Guid? TagId,
    DateOnly? DateFrom,
    DateOnly? DateTo,
    int Page = 1,
    int PageSize = 20);

public sealed record TransactionResponse(
    Guid TransactionId,
    string Type,
    string TypeName,
    decimal Amount,
    DateOnly Date,
    string? Description,
    string Status,
    string StatusName,
    bool IsInstallment,
    int? InstallmentNumber,
    int? TotalInstallments,
    Guid AccountId,
    string AccountName,
    Guid? DestinationAccountId,
    string? DestinationAccountName,
    Guid? CreditCardId,
    string? CreditCardName,
    Guid CategoryId,
    string CategoryName,
    string CategoryIcon,
    string CategoryColor,
    List<TagSummary> Tags,
    DateTime CreatedAt);

public sealed record TagSummary(Guid TagId, string Name, string Color);

public sealed record PagedResponse<T>(List<T> Data, int Page, int PageSize, int Total);
