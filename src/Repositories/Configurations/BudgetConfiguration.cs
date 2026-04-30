using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budget", schema: "Financial");

        builder.HasKey(b => b.BudgetId)
               .HasName("PK_Budget");

        builder.Property(b => b.LimitAmount)
               .HasPrecision(18, 2);

        builder.Property(b => b.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.UpdatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(b => b.User)
               .WithMany(u => u.Budgets)
               .HasForeignKey(b => b.UserId)
               .HasConstraintName("FK_Budget_User")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Category)
               .WithMany(c => c.Budgets)
               .HasForeignKey(b => b.CategoryId)
               .HasConstraintName("FK_Budget_Category")
               .OnDelete(DeleteBehavior.Restrict);

        // One budget per user per category per month/year
        builder.HasIndex(b => new { b.UserId, b.CategoryId, b.Year, b.Month })
               .IsUnique()
               .HasDatabaseName("UQ_Budget_UserCategoryYearMonth");
    }
}
