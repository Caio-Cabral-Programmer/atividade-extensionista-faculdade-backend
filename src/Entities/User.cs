namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class User
{
    public Guid UserId { get; private set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? EmailConfirmationToken { get; set; }
    public DateTime? EmailConfirmationTokenExpiresAt { get; set; }
    public string? TwoFactorCode { get; set; }
    public DateTime? TwoFactorCodeExpiresAt { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiresAt { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Account> Accounts { get; set; } = [];
    public ICollection<CreditCard> CreditCards { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = [];
    public ICollection<Budget> Budgets { get; set; } = [];
    public ICollection<FinancialGoal> FinancialGoals { get; set; } = [];
}
