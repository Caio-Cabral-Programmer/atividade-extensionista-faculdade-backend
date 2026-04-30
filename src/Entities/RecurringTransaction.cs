using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;

namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class RecurringTransaction
{
    public Guid RecurringTransactionId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public RecurrenceFrequency Frequency { get; set; }
    public int? DayOfMonth { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<Transaction> GeneratedTransactions { get; set; } = [];
}
