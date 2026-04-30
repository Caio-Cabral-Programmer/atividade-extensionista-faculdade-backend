namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class Tag
{
    public Guid TagId { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<TransactionTag> TransactionTags { get; set; } = [];
}
