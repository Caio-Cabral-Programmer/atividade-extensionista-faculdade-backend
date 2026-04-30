using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;

namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class Account
{
    public Guid AccountId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public AccountType Type { get; set; }
    public decimal InitialBalance { get; set; }
    public required string Color { get; set; }
    public required string Icon { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Transaction> IncomingTransfers { get; set; } = [];
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = [];
}
