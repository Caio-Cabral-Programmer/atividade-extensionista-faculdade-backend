using AtividadeExtensionistaFaculdadeBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtividadeExtensionistaFaculdadeBackend.Repositories.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
       public void Configure(EntityTypeBuilder<Category> builder)
       {
              builder.ToTable("Category", schema: "Financial");

              builder.HasKey(c => c.CategoryId)
                     .HasName("PK_Category");

              builder.Property(c => c.Name)
                     .IsRequired()
                     .HasMaxLength(100);

              builder.Property(c => c.Type)
                     .HasConversion<string>()
                     .HasMaxLength(30);

              builder.Property(c => c.Icon)
                     .IsRequired()
                     .HasMaxLength(50);

              builder.Property(c => c.Color)
                     .IsRequired()
                     .HasMaxLength(10);

              builder.Property(c => c.CreatedAt)
                     .HasDefaultValueSql("GETUTCDATE()");

              builder.HasOne(c => c.User)
                     .WithMany(u => u.Categories)
                     .HasForeignKey(c => c.UserId)
                     .HasConstraintName("FK_Category_User")
                     .OnDelete(DeleteBehavior.Restrict)
                     .IsRequired(false);

              builder.HasOne(c => c.ParentCategory)
                     .WithMany(c => c.SubCategories)
                     .HasForeignKey(c => c.ParentCategoryId)
                     .HasConstraintName("FK_Category_ParentCategory")
                     .OnDelete(DeleteBehavior.Restrict)
                     .IsRequired(false);

              builder.HasIndex(c => c.UserId)
                     .HasDatabaseName("IX_Category_UserId");

              builder.HasIndex(c => c.ParentCategoryId)
                     .HasDatabaseName("IX_Category_ParentCategoryId");
       }
}
