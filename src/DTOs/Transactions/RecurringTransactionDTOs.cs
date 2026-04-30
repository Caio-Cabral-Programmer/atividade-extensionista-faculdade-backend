namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;

public sealed record CreateRecurringTransactionRequest(
    int Type,
    decimal Amount,
    Guid AccountId,
    Guid CategoryId,
    string? Description,
    int Frequency,
    int? DayOfMonth,
    DateOnly StartDate,
    DateOnly? EndDate);

public sealed record UpdateRecurringTransactionRequest(
    decimal Amount,
    Guid AccountId,
    Guid CategoryId,
    string? Description,
    int Frequency,
    int? DayOfMonth,
    DateOnly? EndDate,
    bool IsActive);

public sealed record RecurringTransactionResponse(
    Guid RecurringTransactionId,
    int Type,
    string TypeName,
    decimal Amount,
    string? Description,
    int Frequency,
    string FrequencyName,
    int? DayOfMonth,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsActive,
    Guid AccountId,
    string AccountName,
    Guid CategoryId,
    string CategoryName);
