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
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Evidence>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("evidence");

            entity.HasIndex(e => e.DiscordUserId, "DiscordUserID");

            entity.HasIndex(e => e.TileId, "TileID");

            entity.Property(e => e.RowId).HasColumnName("RowID");

            entity.Property(e => e.DiscordUserId).HasColumnName("DiscordUserID");

            entity.Property(e => e.TileId).HasColumnName("TileID");

            entity.Property(e => e.EvidenceType).HasColumnName("Type");

            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("URL");

            entity.HasOne(d => d.DiscordUser)
                .WithMany(p => p.Evidence)
                .HasForeignKey(d => d.DiscordUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evidence_ibfk_2");

            entity.HasOne(d => d.Tile)
                .WithMany(p => p.Evidences)
                .HasForeignKey(d => d.TileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evidence_ibfk_1");
        });

        modelBuilder.Entity<Restriction>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("restriction");

            entity.HasIndex(e => e.Name, "Name")
                .IsUnique();

            entity.Property(e => e.RowId).HasColumnName("RowID");

            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<BingoTask>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("task");

            entity.Property(e => e.RowId).HasColumnName("RowID");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Restrictions)
                .WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "Taskrestriction",
                    l => l.HasOne<Restriction>().WithMany().HasForeignKey("RestrictionId").HasConstraintName("Constr_TaskRestriction_Restriction_fk"),
                    r => r.HasOne<BingoTask>().WithMany().HasForeignKey("TaskId").HasConstraintName("Constr_TaskRestriction_Task_fk"),
                    j =>
                    {
                        j.HasKey("TaskId", "RestrictionId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                        j.ToTable("taskrestriction");

                        j.HasIndex(new[] { "RestrictionId" }, "Constr_TaskRestriction_Restriction_fk");

                        j.IndexerProperty<int>("TaskId").HasColumnName("TaskID");

                        j.IndexerProperty<int>("RestrictionId").HasColumnName("RestrictionID");
                    });
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("team");

            entity.Property(e => e.RowId).HasColumnName("RowID");

            entity.Property(e => e.BoardChannelId).HasColumnName("BoardChannelID");

            entity.Property(e => e.BoardMessageId).HasColumnName("BoardMessageID");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tile>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("tile");

            entity.HasIndex(e => new { e.TaskId, e.TeamId }, "task_team_relationship")
                .IsUnique();

            entity.HasIndex(e => new { e.BoardIndex, e.TeamId }, "BoardIndex_team_relationship")
                .IsUnique();

            entity.Property(e => e.RowId).HasColumnName("RowID");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.Property(e => e.BoardIndex).HasColumnName("BoardIndex");

            entity.HasOne(d => d.Task)
                .WithMany(p => p.Tiles)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tile_ibfk_2");

            entity.HasOne(d => d.Team)
                .WithMany(p => p.Tiles)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tile_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.DiscordUserId)
                .HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.TeamId, "TeamID");

            entity.Property(e => e.DiscordUserId)
                .ValueGeneratedNever()
                .HasColumnName("DiscordUserID");

            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.Team)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
