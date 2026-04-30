using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", schema: "Identity");

        builder.HasKey(u => u.UserId)
               .HasName("PK_User");

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(512);

        builder.Property(u => u.EmailConfirmationToken)
               .HasMaxLength(512);

        builder.Property(u => u.TwoFactorCode)
               .HasMaxLength(10);

        builder.Property(u => u.PasswordResetToken)
               .HasMaxLength(512);

        builder.Property(u => u.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UpdatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(u => u.Email)
               .IsUnique()
               .HasDatabaseName("UQ_User_Email");
    }
}
