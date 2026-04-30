using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class TransactionTagConfiguration : IEntityTypeConfiguration<TransactionTag>
{
    public void Configure(EntityTypeBuilder<TransactionTag> builder)
    {
        builder.ToTable("TransactionTag", schema: "Financial");

        builder.HasKey(tt => new { tt.TransactionId, tt.TagId })
               .HasName("PK_TransactionTag");

        builder.HasOne(tt => tt.Transaction)
               .WithMany(t => t.TransactionTags)
               .HasForeignKey(tt => tt.TransactionId)
               .HasConstraintName("FK_TransactionTag_Transaction")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tt => tt.Tag)
               .WithMany(t => t.TransactionTags)
               .HasForeignKey(tt => tt.TagId)
               .HasConstraintName("FK_TransactionTag_Tag")
               .OnDelete(DeleteBehavior.Restrict);
    }
}
