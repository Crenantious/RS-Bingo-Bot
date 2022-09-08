﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RSBingo_Framework.Models;

namespace RSBingo_Framework.DAL;

public partial class RSBingoContext : DbContext
{
    public RSBingoContext(DbContextOptions options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Evidence>(entity =>
        {
            entity.HasKey(e => e.Rowid)
                .HasName("PRIMARY");

            entity.ToTable("evidence");

            entity.HasIndex(e => e.Rowid, "taskID_idx");

            entity.Property(e => e.Rowid).ValueGeneratedNever();

            entity.Property(e => e.LocationUrl)
                .HasMaxLength(512)
                .HasColumnName("LocationURL");

            entity.Property(e => e.TileId).HasColumnName("TileID");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Row)
                .WithOne(p => p.Evidence)
                .HasForeignKey<Evidence>(d => d.Rowid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TileID");

            entity.HasOne(d => d.RowNavigation)
                .WithOne(p => p.Evidence)
                .HasForeignKey<Evidence>(d => d.Rowid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserID");
        });

        modelBuilder.Entity<Restrciton>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("restrcitons");

            entity.Property(e => e.RowId)
                .ValueGeneratedNever()
                .HasColumnName("RowID");

            entity.Property(e => e.Description).HasMaxLength(512);
        });

        modelBuilder.Entity<BingoTask>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("tasks");

            entity.Property(e => e.RowId)
                .ValueGeneratedNever()
                .HasColumnName("RowID");

            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<Taskresriction>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("taskresrictions");

            entity.HasIndex(e => e.RestrictionId, "restrictionID_idx");

            entity.HasIndex(e => e.TaskId, "taskID_idx");

            entity.Property(e => e.RestrictionId).HasColumnName("RestrictionID");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.HasOne(d => d.Restriction)
                .WithMany()
                .HasForeignKey(d => d.RestrictionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RestrictionID");

            entity.HasOne(d => d.Task)
                .WithMany()
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TaskID");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("teams");

            entity.Property(e => e.RowId)
                .ValueGeneratedNever()
                .HasColumnName("RowID");

            entity.Property(e => e.TeamName).HasMaxLength(45);
        });

        modelBuilder.Entity<Tile>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("tile");

            entity.Property(e => e.RowId)
                .ValueGeneratedNever()
                .HasColumnName("RowID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.TeamId, "rowid_idx");

            entity.Property(e => e.RowId)
                .ValueGeneratedNever()
                .HasColumnName("RowID");

            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Team)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rowid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
