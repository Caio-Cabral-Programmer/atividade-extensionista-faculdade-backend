using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class FinancialGoalConfiguration : IEntityTypeConfiguration<FinancialGoal>
{
    public void Configure(EntityTypeBuilder<FinancialGoal> builder)
    {
        builder.ToTable("FinancialGoal", schema: "Financial");

        builder.HasKey(g => g.FinancialGoalId)
               .HasName("PK_FinancialGoal");

        builder.Property(g => g.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(g => g.TargetAmount)
               .HasPrecision(18, 2);

        builder.Property(g => g.CurrentAmount)
               .HasPrecision(18, 2);

        builder.Property(g => g.Description)
               .HasMaxLength(500);

        builder.Property(g => g.Icon)
               .HasMaxLength(50);

        builder.Property(g => g.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(g => g.UpdatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(g => g.User)
               .WithMany(u => u.FinancialGoals)
               .HasForeignKey(g => g.UserId)
               .HasConstraintName("FK_FinancialGoal_User")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => g.UserId)
               .HasDatabaseName("IX_FinancialGoal_UserId");
    }
}
