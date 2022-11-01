// <copyright file="RSBingoContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DAL;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RSBingo_Framework.Models;

public partial class RSBingoContext : DbContext
{
     // TODO: JCH - Need to see if the DB auto handles create PK on save. If not need to update the DB to AUTO_INCREMENT.

    public RSBingoContext(DbContextOptions options)
        : base(options) { }

    /// <summary>
    /// Request to undo all changed to any entity that are marked with a change.
    /// </summary>
    public void RollBack()
    {
        List<EntityEntry> changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

        foreach (EntityEntry entry in changedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }

    /// <inheritdoc/>
    public override int SaveChanges()
    {
        // Check for custom validation
        IEnumerable<EntityEntry> recordsToValidate = ChangeTracker.Entries();

        foreach (EntityEntry recordToValidate in recordsToValidate)
        {
            // Perform validation based on EntityState and recordToValidate.Entity type.
        }

        return base.SaveChanges();
    }

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

            entity.HasOne(d => d.Tile)
                .WithMany(p => p.Evidence)
                .HasForeignKey(d => d.TileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Evidence_TileID");
        });

        modelBuilder.Entity<Restrciton>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("restrictions");

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

            entity.Property(e => e.Image)
                .HasColumnName("Image");

            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<TaskRestrciton>(entity =>
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

            entity.Property(e => e.Name).HasMaxLength(45);
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

            entity.HasMany(d => d.Evidence)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
