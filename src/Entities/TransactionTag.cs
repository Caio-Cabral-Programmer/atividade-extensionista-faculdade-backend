namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class TransactionTag
{
    public Guid TransactionId { get; set; }
    public Guid TagId { get; set; }

    // Navigation
    public Transaction Transaction { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
