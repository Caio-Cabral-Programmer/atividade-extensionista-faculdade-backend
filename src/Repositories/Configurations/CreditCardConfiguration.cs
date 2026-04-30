using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        builder.ToTable("CreditCard", schema: "Financial");

        builder.HasKey(c => c.CreditCardId)
               .HasName("PK_CreditCard");

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.CreditLimit)
               .HasPrecision(18, 2);

        builder.Property(c => c.Color)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.UpdatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(c => c.User)
               .WithMany(u => u.CreditCards)
               .HasForeignKey(c => c.UserId)
               .HasConstraintName("FK_CreditCard_User")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.UserId)
               .HasDatabaseName("IX_CreditCard_UserId");
    }
}
