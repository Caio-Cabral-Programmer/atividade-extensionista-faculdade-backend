namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class Budget
{
    public Guid BudgetId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal LimitAmount { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
