namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;

public sealed record CreateAccountRequest(
    string Name,
    string Type,
    decimal InitialBalance,
    string Color,
    string Icon);

public sealed record UpdateAccountRequest(
    string Name,
    string Type,
    decimal InitialBalance,
    string Color,
    string Icon,
    bool IsActive);

public sealed record AccountResponse(
    Guid AccountId,
    string Name,
    string Type,
    string TypeName,
    decimal InitialBalance,
    decimal CurrentBalance,
    string Color,
    string Icon,
    bool IsActive,
    DateTime CreatedAt);
