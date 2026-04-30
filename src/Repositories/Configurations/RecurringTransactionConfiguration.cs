using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
       public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
       {
              builder.ToTable("RecurringTransaction", schema: "Financial");

              builder.HasKey(r => r.RecurringTransactionId)
                     .HasName("PK_RecurringTransaction");

              builder.Property(r => r.Amount)
                     .HasPrecision(18, 2);

              builder.Property(r => r.Type)
                     .HasConversion<string>()
                     .HasMaxLength(30);

              builder.Property(r => r.Frequency)
                     .HasConversion<string>()
                     .HasMaxLength(30);

              builder.Property(r => r.Description)
                     .HasMaxLength(500);

              builder.Property(r => r.CreatedAt)
                     .HasDefaultValueSql("GETUTCDATE()");

              builder.Property(r => r.UpdatedAt)
                     .HasDefaultValueSql("GETUTCDATE()");

              builder.HasOne(r => r.User)
                     .WithMany(u => u.RecurringTransactions)
                     .HasForeignKey(r => r.UserId)
                     .HasConstraintName("FK_RecurringTransaction_User")
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(r => r.Account)
                     .WithMany(a => a.RecurringTransactions)
                     .HasForeignKey(r => r.AccountId)
                     .HasConstraintName("FK_RecurringTransaction_Account")
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(r => r.Category)
                     .WithMany(c => c.RecurringTransactions)
                     .HasForeignKey(r => r.CategoryId)
                     .HasConstraintName("FK_RecurringTransaction_Category")
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasIndex(r => r.UserId)
                     .HasDatabaseName("IX_RecurringTransaction_UserId");
       }
}
