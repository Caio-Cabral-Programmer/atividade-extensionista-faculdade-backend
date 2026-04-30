using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tag", schema: "Financial");

        builder.HasKey(t => t.TagId)
               .HasName("PK_Tag");

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(t => t.Color)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(t => t.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(t => t.User)
               .WithMany(u => u.Tags)
               .HasForeignKey(t => t.UserId)
               .HasConstraintName("FK_Tag_User")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => t.UserId)
               .HasDatabaseName("IX_Tag_UserId");
    }
}
