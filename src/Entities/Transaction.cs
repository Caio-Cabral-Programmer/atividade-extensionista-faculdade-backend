using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;

namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class Transaction
{
    public Guid TransactionId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public Guid? DestinationAccountId { get; set; }
    public Guid? CreditCardId { get; set; }
    public Guid CategoryId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public string? Description { get; set; }
    public TransactionStatus Status { get; set; }
    public bool IsInstallment { get; set; }
    public int? InstallmentNumber { get; set; }
    public int? TotalInstallments { get; set; }
    public Guid? InstallmentGroupId { get; set; }
    public Guid? RecurringTransactionId { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public Account Account { get; set; } = null!;
    public Account? DestinationAccount { get; set; }
    public CreditCard? CreditCard { get; set; }
    public Category Category { get; set; } = null!;
    public RecurringTransaction? RecurringTransaction { get; set; }
    public ICollection<TransactionTag> TransactionTags { get; set; } = [];
}
