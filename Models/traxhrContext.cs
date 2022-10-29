using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TraxHrPolicy.Models
{
    public partial class traxhrContext : DbContext
    {
        public traxhrContext()
        {
        }

        public traxhrContext(DbContextOptions<traxhrContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Policy> Policies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>(entity =>
            {
                entity.ToTable("policies");

                entity.HasIndex(e => e.Id1, "policies__id_unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("bigint unsigned")
                    .HasColumnName("category_id");

                entity.Property(e => e.ChangeType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("change_type")
                    .HasDefaultValueSql("'minor'");

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("content_type")
                    .HasDefaultValueSql("'pdf'");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasColumnType("longtext")
                    .HasColumnName("description");

                entity.Property(e => e.Icon)
                    .HasMaxLength(255)
                    .HasColumnName("icon");

                entity.Property(e => e.Id1)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("_id")
                    .IsFixedLength(true);

                entity.Property(e => e.Parent)
                    .HasColumnName("parent")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ParentType)
                    .HasMaxLength(255)
                    .HasColumnName("parent_type");

                entity.Property(e => e.Published)
                    .IsRequired()
                    .HasColumnName("published")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Quiz).HasColumnName("quiz");

                entity.Property(e => e.ShortDescription)
                    .HasColumnType("longtext")
                    .HasColumnName("short_description");

                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .HasColumnName("slug");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.TotalVersions)
                    .HasColumnName("total_versions")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'policy'");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("version")
                    .HasDefaultValueSql("'1.0.0'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
