namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class CreditCard
{
    public Guid CreditCardId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public decimal CreditLimit { get; set; }
    public int ClosingDay { get; set; }
    public int DueDay { get; set; }
    public required string Color { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
