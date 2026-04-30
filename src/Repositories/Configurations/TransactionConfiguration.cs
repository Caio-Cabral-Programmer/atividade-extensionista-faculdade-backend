using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction", schema: "Financial");

        builder.HasKey(t => t.TransactionId)
               .HasName("PK_Transaction");

        builder.Property(t => t.Amount)
               .HasPrecision(18, 2);

        builder.Property(t => t.Description)
               .HasMaxLength(500);

        builder.Property(t => t.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.UpdatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(t => t.User)
               .WithMany(u => u.Transactions)
               .HasForeignKey(t => t.UserId)
               .HasConstraintName("FK_Transaction_User")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Account)
               .WithMany(a => a.Transactions)
               .HasForeignKey(t => t.AccountId)
               .HasConstraintName("FK_Transaction_Account")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.DestinationAccount)
               .WithMany(a => a.IncomingTransfers)
               .HasForeignKey(t => t.DestinationAccountId)
               .HasConstraintName("FK_Transaction_DestinationAccount")
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(false);

        builder.HasOne(t => t.CreditCard)
               .WithMany(c => c.Transactions)
               .HasForeignKey(t => t.CreditCardId)
               .HasConstraintName("FK_Transaction_CreditCard")
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(false);

        builder.HasOne(t => t.Category)
               .WithMany(c => c.Transactions)
               .HasForeignKey(t => t.CategoryId)
               .HasConstraintName("FK_Transaction_Category")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.RecurringTransaction)
               .WithMany(r => r.GeneratedTransactions)
               .HasForeignKey(t => t.RecurringTransactionId)
               .HasConstraintName("FK_Transaction_RecurringTransaction")
               .OnDelete(DeleteBehavior.SetNull)
               .IsRequired(false);

        builder.HasIndex(t => t.UserId)
               .HasDatabaseName("IX_Transaction_UserId");

        builder.HasIndex(t => t.AccountId)
               .HasDatabaseName("IX_Transaction_AccountId");

        builder.HasIndex(t => new { t.UserId, t.Date })
               .HasDatabaseName("IX_Transaction_UserIdDate");

        builder.HasIndex(t => t.InstallmentGroupId)
               .HasDatabaseName("IX_Transaction_InstallmentGroupId");
    }
}
