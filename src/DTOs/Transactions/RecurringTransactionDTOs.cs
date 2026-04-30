namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;

public sealed record CreateRecurringTransactionRequest(
    string Type,
    decimal Amount,
    Guid AccountId,
    Guid CategoryId,
    string? Description,
    string Frequency,
    int? DayOfMonth,
    DateOnly StartDate,
    DateOnly? EndDate);

public sealed record UpdateRecurringTransactionRequest(
    decimal Amount,
    Guid AccountId,
    Guid CategoryId,
    string? Description,
    string Frequency,
    int? DayOfMonth,
    DateOnly? EndDate,
    bool IsActive);

public sealed record RecurringTransactionResponse(
    Guid RecurringTransactionId,
    string Type,
    string TypeName,
    decimal Amount,
    string? Description,
    string Frequency,
    string FrequencyName,
    int? DayOfMonth,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsActive,
    Guid AccountId,
    string AccountName,
    Guid CategoryId,
    string CategoryName);
