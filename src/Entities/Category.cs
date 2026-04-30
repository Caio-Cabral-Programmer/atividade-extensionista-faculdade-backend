using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;

namespace AtividadeExtensionistaFaculdadeBackend.Entities;

public sealed class Category
{
    public Guid CategoryId { get; private set; } = Guid.NewGuid();
    public Guid? UserId { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public required string Name { get; set; }
    public CategoryType Type { get; set; }
    public required string Icon { get; set; }
    public required string Color { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Navigation
    public User? User { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = [];
    public ICollection<Budget> Budgets { get; set; } = [];
}
