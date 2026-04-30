namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;

public sealed record CreateAccountRequest(
    string Name,
    int Type,
    decimal InitialBalance,
    string Color,
    string Icon);

public sealed record UpdateAccountRequest(
    string Name,
    int Type,
    decimal InitialBalance,
    string Color,
    string Icon,
    bool IsActive);

public sealed record AccountResponse(
    Guid AccountId,
    string Name,
    int Type,
    string TypeName,
    decimal InitialBalance,
    decimal CurrentBalance,
    string Color,
    string Icon,
    bool IsActive,
    DateTime CreatedAt);
