namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Goals;

public sealed record CreateGoalRequest(
    string Name,
    decimal TargetAmount,
    DateOnly? Deadline,
    string? Description,
    string? Icon);

public sealed record UpdateGoalRequest(
    string Name,
    decimal TargetAmount,
    DateOnly? Deadline,
    string? Description,
    string? Icon);

public sealed record GoalDepositWithdrawRequest(decimal Amount);

public sealed record GoalResponse(
    Guid FinancialGoalId,
    string Name,
    decimal TargetAmount,
    decimal CurrentAmount,
    decimal RemainingAmount,
    double ProgressPercent,
    DateOnly? Deadline,
    string? Description,
    string? Icon,
    bool IsCompleted,
    DateTime CreatedAt);
