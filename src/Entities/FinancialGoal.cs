namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class FinancialGoal
{
    public Guid FinancialGoalId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateOnly? Deadline { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}
