using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account", schema: "Financial");

        builder.HasKey(a => a.AccountId)
               .HasName("PK_Account");

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.InitialBalance)
               .HasPrecision(18, 2);

        builder.Property(a => a.Color)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(a => a.Icon)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(a => a.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.UpdatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(a => a.User)
               .WithMany(u => u.Accounts)
               .HasForeignKey(a => a.UserId)
               .HasConstraintName("FK_Account_User")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.UserId)
               .HasDatabaseName("IX_Account_UserId");
    }
}
