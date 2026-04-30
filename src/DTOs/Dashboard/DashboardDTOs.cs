namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Dashboard;

public sealed record DashboardSummaryResponse(
    decimal TotalBalance,
    decimal MonthIncome,
    decimal MonthExpenses,
    decimal BillsDueNext7Days,
    int PendingTransactionsCount);

public sealed record ExpensesByCategoryItem(
    Guid CategoryId,
    string CategoryName,
    string CategoryColor,
    decimal TotalAmount,
    double Percentage);

public sealed record MonthlyBalanceItem(int Year, int Month, string MonthName, decimal Income, decimal Expenses, decimal NetBalance);

public sealed record BalanceProjectionResponse(
    decimal CurrentBalance,
    decimal PendingExpenses,
    decimal PendingIncome,
    decimal ProjectedBalance);
