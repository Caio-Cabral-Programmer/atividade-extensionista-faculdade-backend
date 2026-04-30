using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<CreditCard> CreditCards => Set<CreditCard>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionTag> TransactionTags => Set<TransactionTag>();
    public DbSet<RecurringTransaction> RecurringTransactions => Set<RecurringTransaction>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<FinancialGoal> FinancialGoals => Set<FinancialGoal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var updatedAtProperty = entry.Properties
                .FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");

            if (updatedAtProperty is not null)
                updatedAtProperty.CurrentValue = DateTime.UtcNow;
        }
    }
}
