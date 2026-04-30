namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Budgets;

public sealed record CreateBudgetRequest(Guid CategoryId, int Year, int Month, decimal LimitAmount);
public sealed record UpdateBudgetRequest(decimal LimitAmount);

public sealed record BudgetResponse(
    Guid BudgetId,
    Guid CategoryId,
    string CategoryName,
    string CategoryIcon,
    string CategoryColor,
    int Year,
    int Month,
    decimal LimitAmount,
    decimal SpentAmount,
    decimal RemainingAmount,
    double ProgressPercent);
